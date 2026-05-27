using Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;


namespace PlantifyAdmin.Services;

public class EmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendOrderConfirmationAsync(OrderModel order)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        message.To.Add(new MailboxAddress($"{order.FirstName} {order.LastName}", order.Email));
        message.Subject = $"🌿 Order Confirmed – Plantify #{order.Id?.Substring(0, 8).ToUpper()}";

        message.Body = new TextPart("html")
        {
            Text = BuildEmailHtml(order)
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendDriveConfirmationAsync(Plantation drive, ParticipantInput participant)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        message.To.Add(new MailboxAddress($"{participant.FirstName} {participant.LastName}", participant.Email));
        message.Subject = $"🌱 You're In! – {drive.Title} | Plantify";

        message.Body = new TextPart("html")
        {
            Text = BuildDriveEmailHtml(drive, participant)
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    private string BuildDriveEmailHtml(Plantation drive, ParticipantInput participant)
    {
        return $"""
    <!DOCTYPE html>
    <html>
    <head><meta charset="utf-8"/></head>
    <body style="margin:0;padding:0;background:#f4f7f3;font-family:'Segoe UI',Arial,sans-serif;">
        <div style="max-width:620px;margin:40px auto;background:white;border-radius:24px;overflow:hidden;box-shadow:0 10px 40px rgba(0,0,0,0.08);">

            <!-- HEADER -->
            <div style="background:linear-gradient(135deg,#43a047,#2e7d32);padding:40px;text-align:center;">
                <div style="font-size:52px;margin-bottom:12px;">🌱</div>
                <h1 style="color:white;margin:0;font-size:26px;font-weight:900;">You're Joining the Drive!</h1>
                <p style="color:rgba(255,255,255,0.85);margin:10px 0 0;font-size:15px;">
                    Hi {participant.FirstName}, your spot is confirmed.
                </p>
            </div>

            <!-- BODY -->
            <div style="padding:36px 40px;">

                <!-- DRIVE CARD -->
                <div style="background:#f0fdf4;border-radius:20px;padding:28px;margin-bottom:28px;border-left:5px solid #2e7d32;">
                    <h2 style="margin:0 0 20px;font-size:22px;font-weight:800;color:#1b5e20;">
                        {drive.Title}
                    </h2>

                    <table style="width:100%;border-collapse:collapse;">
                        <tr>
                            <td style="padding:10px 0;border-bottom:1px solid #dcfce7;width:40%;">
                                <span style="font-size:18px;">📍</span>
                                <span style="color:#6b7280;font-size:13px;margin-left:8px;">Location</span>
                            </td>
                            <td style="padding:10px 0;border-bottom:1px solid #dcfce7;font-weight:700;color:#111;">
                                {drive.Location}
                            </td>
                        </tr>
                        <tr>
                            <td style="padding:10px 0;border-bottom:1px solid #dcfce7;">
                                <span style="font-size:18px;">📅</span>
                                <span style="color:#6b7280;font-size:13px;margin-left:8px;">Date</span>
                            </td>
                            <td style="padding:10px 0;border-bottom:1px solid #dcfce7;font-weight:700;color:#111;">
                                {drive.Date:dddd, MMMM dd, yyyy}
                            </td>
                        </tr>
                        <tr>
                            <td style="padding:10px 0;border-bottom:1px solid #dcfce7;">
                                <span style="font-size:18px;">🤝</span>
                                <span style="color:#6b7280;font-size:13px;margin-left:8px;">Organized By</span>
                            </td>
                            <td style="padding:10px 0;border-bottom:1px solid #dcfce7;font-weight:700;color:#111;">
                                {drive.OrganizedBy}
                            </td>
                        </tr>
                        <tr>
                            <td style="padding:10px 0;">
                                <span style="font-size:18px;">🌳</span>
                                <span style="color:#6b7280;font-size:13px;margin-left:8px;">Saplings Goal</span>
                            </td>
                            <td style="padding:10px 0;font-weight:700;color:#111;">
                                {drive.TotalSaplings} trees
                            </td>
                        </tr>
                    </table>
                </div>

                <!-- PARTICIPANT DETAILS -->
                <h3 style="font-size:15px;font-weight:700;color:#111;margin:0 0 14px;">👤 Your Registration</h3>
                <div style="background:#f9fafb;border-radius:16px;padding:20px 24px;margin-bottom:28px;">
                    <div style="display:flex;justify-content:space-between;margin-bottom:8px;">
                        <span style="color:#6b7280;">Name</span>
                        <span style="font-weight:600;">{participant.FirstName} {participant.LastName}</span>
                    </div>
                    <div style="display:flex;justify-content:space-between;margin-bottom:8px;">
                        <span style="color:#6b7280;">Email</span>
                        <span style="font-weight:600;">{participant.Email}</span>
                    </div>
                    <div style="display:flex;justify-content:space-between;">
                        <span style="color:#6b7280;">Phone</span>
                        <span style="font-weight:600;">{participant.PhoneNumber}</span>
                    </div>
                </div>

                <!-- REMINDER BOX -->
                <div style="background:#fffbeb;border-radius:16px;padding:20px 24px;margin-bottom:28px;border-left:4px solid #f59e0b;">
                    <p style="margin:0;font-weight:700;color:#92400e;margin-bottom:8px;">📌 What to bring</p>
                    <ul style="margin:0;padding-left:20px;color:#78350f;line-height:1.9;font-size:14px;">
                        <li>Comfortable outdoor clothing</li>
                        <li>Water bottle &amp; sunscreen</li>
                        <li>Gardening gloves if you have them</li>
                        <li>Enthusiasm to plant trees! 🌿</li>
                    </ul>
                </div>

                <p style="color:#6b7280;font-size:13px;line-height:1.7;text-align:center;margin:0;">
                    See you at the drive, {participant.FirstName}! 🌍<br/>
                    <strong style="color:#2e7d32;">Plantify</strong> — Together We Grow
                </p>
            </div>

            <!-- FOOTER -->
            <div style="background:#f0fdf4;padding:20px 40px;text-align:center;border-top:1px solid #e5e7eb;">
                <p style="color:#9ca3af;font-size:12px;margin:0;">
                    © 2025 Plantify. You received this because you registered for a plantation drive.
                </p>
            </div>

        </div>
    </body>
    </html>
    """;
    }

    private string BuildEmailHtml(OrderModel order)
    {
        var gstRate = order.PaymentMethod == "Cash on Delivery" ? 15 : 5;
        var itemsHtml = string.Join("", order.Items?.Select(item => $"""
            <tr>
                <td style="padding:12px 16px; border-bottom:1px solid #e5e7eb;">
                    <strong>{item.PlantName}</strong>
                </td>
                <td style="padding:12px 16px; border-bottom:1px solid #e5e7eb; text-align:center;">
                    {item.Quantity}
                </td>
                <td style="padding:12px 16px; border-bottom:1px solid #e5e7eb; text-align:right;">
                    Rs {item.Price:F0}
                </td>
                <td style="padding:12px 16px; border-bottom:1px solid #e5e7eb; text-align:right;">
                    Rs {(item.Price * item.Quantity):F0}
                </td>
            </tr>
        """) ?? []);

        return $"""
        <!DOCTYPE html>
        <html>
        <head><meta charset="utf-8"/></head>
        <body style="margin:0;padding:0;background:#f4f7f3;font-family:'Segoe UI',Arial,sans-serif;">
            <div style="max-width:620px;margin:40px auto;background:white;border-radius:24px;overflow:hidden;box-shadow:0 10px 40px rgba(0,0,0,0.08);">

                <!-- HEADER -->
                <div style="background:linear-gradient(135deg,#43a047,#2e7d32);padding:40px 40px 32px;text-align:center;">
                    <div style="font-size:48px;margin-bottom:12px;">🌿</div>
                    <h1 style="color:white;margin:0;font-size:28px;font-weight:900;letter-spacing:-1px;">Order Confirmed!</h1>
                    <p style="color:rgba(255,255,255,0.85);margin:10px 0 0;font-size:15px;">
                        Thank you, {order.FirstName}! Your plants are on their way.
                    </p>
                </div>

                <!-- BODY -->
                <div style="padding:36px 40px;">

                    <!-- ORDER META -->
                    <div style="background:#f0fdf4;border-radius:16px;padding:20px 24px;margin-bottom:28px;">
                        <table style="width:100%;border-collapse:collapse;">
                            <tr>
                                <td style="font-size:13px;color:#6b7280;">Order ID</td>
                                <td style="font-size:13px;color:#6b7280;">Payment Method</td>
                                <td style="font-size:13px;color:#6b7280;">Delivery To</td>
                            </tr>
                            <tr>
                                <td style="font-weight:700;color:#1b5e20;padding-top:6px;">
                                    #{order.Id?.Substring(0, 8).ToUpper()}
                                </td>
                                <td style="font-weight:700;color:#1b5e20;padding-top:6px;">
                                    {order.PaymentMethod}
                                </td>
                                <td style="font-weight:700;color:#1b5e20;padding-top:6px;">
                                    {order.City}
                                </td>
                            </tr>
                        </table>
                    </div>

                    <!-- SHIPPING ADDRESS -->
                    <h3 style="font-size:16px;font-weight:700;color:#111;margin:0 0 12px;">📦 Shipping Address</h3>
                    <p style="color:#374151;line-height:1.7;margin:0 0 28px;">
                        {order.FirstName} {order.LastName}<br/>
                        {order.Address}<br/>
                        {order.City}{(string.IsNullOrWhiteSpace(order.PostalCode) ? "" : $", {order.PostalCode}")}<br/>
                        📞 {order.Phone}
                    </p>

                    <!-- ITEMS TABLE -->
                    <h3 style="font-size:16px;font-weight:700;color:#111;margin:0 0 12px;">🌱 Order Items</h3>
                    <table style="width:100%;border-collapse:collapse;margin-bottom:24px;border-radius:16px;overflow:hidden;border:1px solid #e5e7eb;">
                        <thead>
                            <tr style="background:#f9fafb;">
                                <th style="padding:12px 16px;text-align:left;font-size:13px;color:#6b7280;font-weight:600;">Product</th>
                                <th style="padding:12px 16px;text-align:center;font-size:13px;color:#6b7280;font-weight:600;">Qty</th>
                                <th style="padding:12px 16px;text-align:right;font-size:13px;color:#6b7280;font-weight:600;">Unit Price</th>
                                <th style="padding:12px 16px;text-align:right;font-size:13px;color:#6b7280;font-weight:600;">Total</th>
                            </tr>
                        </thead>
                        <tbody>
                            {itemsHtml}
                        </tbody>
                    </table>

                    <!-- RECEIPT BREAKDOWN -->
                    <div style="background:#f9fafb;border-radius:16px;padding:20px 24px;margin-bottom:32px;">
                        <div style="display:flex;justify-content:space-between;margin-bottom:10px;">
                            <span style="color:#374151;">Subtotal</span>
                            <span style="font-weight:600;">Rs {order.SubTotal:F0}</span>
                        </div>
                        <div style="display:flex;justify-content:space-between;margin-bottom:10px;">
                            <span style="color:#374151;">GST ({gstRate}%) — {order.PaymentMethod}</span>
                            <span style="font-weight:600;">Rs {order.Gst:F0}</span>
                        </div>
                        <div style="border-top:2px solid #e5e7eb;margin:14px 0;"></div>
                        <div style="display:flex;justify-content:space-between;">
                            <span style="font-size:17px;font-weight:800;color:#1b5e20;">Final Total</span>
                            <span style="font-size:17px;font-weight:800;color:#1b5e20;">Rs {order.FinalTotal:F0}</span>
                        </div>
                    </div>

                    <!-- FOOTER NOTE -->
                    <p style="color:#6b7280;font-size:13px;line-height:1.7;text-align:center;margin:0;">
                        Questions? Reply to this email or contact us anytime.<br/>
                        <strong style="color:#2e7d32;">Plantify</strong> — Grow Your Dream Garden 🌿
                    </p>

                </div>

                <!-- FOOTER BAR -->
                <div style="background:#f0fdf4;padding:20px 40px;text-align:center;border-top:1px solid #e5e7eb;">
                    <p style="color:#9ca3af;font-size:12px;margin:0;">
                        © 2025 Plantify. This is an automated order confirmation.
                    </p>
                </div>

            </div>
        </body>
        </html>
        """;
    }
}