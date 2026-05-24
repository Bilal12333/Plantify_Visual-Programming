from flask import Flask, request, jsonify
import numpy as np
import cv2
from PIL import Image
import tensorflow as tf
from groq import Groq
import io, base64, json, os

app = Flask(__name__)

GROQ_API_KEY = os.environ.get("GROQ_API_KEY")
client = Groq(api_key=GROQ_API_KEY)

CLASS_NAMES = [
    "Pepper__bell___Bacterial_spot",
    "Pepper__bell___healthy",
    "Potato___Early_blight",
    "Potato___Late_blight",
    "Potato___healthy",
    "Tomato_Bacterial_spot",
    "Tomato_Early_blight",
    "Tomato_Late_blight",
    "Tomato_Leaf_Mold",
    "Tomato_Septoria_leaf_spot",
    "Tomato_Spider_mites_Two_spotted_spider_mite",
    "Tomato__Target_Spot",
    "Tomato__Tomato_YellowLeaf__Curl_Virus",
    "Tomato__Tomato_mosaic_virus",
    "Tomato_healthy"
]

PLANT_CLASS_MAP = {
    "tomato": [5,6,7,8,9,10,11,12,13,14],
    "potato": [2,3,4],
    "pepper": [0,1],
}

model = tf.keras.models.load_model("plant_disease_model_v3.keras")

def clean_class_name(name):
    return name.replace("___"," - ").replace("__"," ").replace("_"," ")

def preprocess_image(image):
    image = np.array(image)
    image = cv2.resize(image, (224, 224))
    image = image.astype(np.float32) / 255.0
    mean = np.array([0.485, 0.456, 0.406])
    std  = np.array([0.229, 0.224, 0.225])
    image = (image - mean) / std
    return np.expand_dims(image, axis=0)

def is_blurry(image, threshold=80):
    gray = cv2.cvtColor(np.array(image), cv2.COLOR_RGB2GRAY)
    return cv2.Laplacian(gray, cv2.CV_64F).var() < threshold

def has_enough_green(image, green_threshold=0.08):
    img = np.array(image)
    r, g, b = img[:,:,0], img[:,:,1], img[:,:,2]
    return (np.sum((g > r) & (g > b)) / img[:,:,0].size) > green_threshold

def image_to_base64(image):
    buf = io.BytesIO()
    image.save(buf, format="JPEG", quality=85)
    buf.seek(0)
    return base64.b64encode(buf.read()).decode("utf-8")

def validate_plant_with_vision(image):
    b64 = image_to_base64(image)
    prompt = """You are a strict botanical expert. Reply ONLY with valid JSON:
{
  "is_supported_plant": true or false,
  "plant_type": "tomato" | "potato" | "pepper" | "other",
  "detected_plant": "<name>",
  "reason": "<one sentence>"
}
Only true for Tomato, Potato, or Pepper Bell LEAF images."""

    try:
        r = client.chat.completions.create(
            model="meta-llama/llama-4-scout-17b-16e-instruct",
            messages=[{"role":"user","content":[
                {"type":"image_url","image_url":{"url":f"data:image/jpeg;base64,{b64}"}},
                {"type":"text","text":prompt}
            ]}],
            max_tokens=250, temperature=0.0
        )
        raw = r.choices[0].message.content.strip()
        if "```" in raw:
            raw = raw.split("```")[1]
            if raw.lower().startswith("json"):
                raw = raw[4:]
        d = json.loads(raw.strip())
        is_valid   = bool(d.get("is_supported_plant", False))
        plant_type = str(d.get("plant_type","other")).lower().strip()
        if is_valid and plant_type not in PLANT_CLASS_MAP:
            is_valid, plant_type = False, "other"
        return is_valid, plant_type, d.get("detected_plant","Unknown"), d.get("reason",""), ""
    except Exception as e:
        return False, "unknown", "Unknown", "Validation failed.", str(e)

def get_ai_analysis(disease_name):
    try:
        cc = client.chat.completions.create(
            messages=[
                {"role":"system","content":"You are PlantAI, a plant disease expert."},
                {"role":"user","content":f"Explain this disease: {disease_name}. Include causes, symptoms, treatment, prevention."}
            ],
            model="llama-3.1-8b-instant"
        )
        return cc.choices[0].message.content
    except Exception as e:
        return f"Analysis failed: {str(e)}"

@app.route("/predict", methods=["POST"])
def predict():
    if "image" not in request.files:
        return jsonify({"error": "No image provided"}), 400

    file  = request.files["image"]
    image = Image.open(io.BytesIO(file.read())).convert("RGB")

    if is_blurry(image):
        return jsonify({"error": "Image is too blurry"}), 400

    if not has_enough_g