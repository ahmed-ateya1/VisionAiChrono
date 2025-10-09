namespace VisionAiChrono.Application.Helper
{
    public static class EmailBodyHelper
    {
        public static string EmailBody(string Title, string content, string email, string otpCode)
        {
            return $@"
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                color: #333;
                line-height: 1.6;
                background-color: #f4f4f4;
                margin: 0;
                padding: 0;
            }}
            .container {{
                max-width: 600px;
                margin: 20px auto;
                padding: 20px;
                border: 1px solid #ddd;
                border-radius: 8px;
                background-color: #ffffff;
                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            }}
            .header {{
                text-align: center;
                padding-bottom: 20px;
            }}
            .header h1 {{
                color: #007BFF;
                font-size: 24px;
                margin: 0;
            }}
            .content {{
                padding: 20px;
                background-color: #ffffff;
                border-radius: 8px;
                text-align: center;
            }}
            .otp-code {{
                font-size: 24px;
                font-weight: bold;
                color: #007BFF;
                margin: 20px 0;
                padding: 10px;
                border: 2px dashed #007BFF;
                display: inline-block;
                letter-spacing: 2px;
            }}
            .footer {{
                text-align: center;
                margin-top: 20px;
                font-size: 12px;
                color: #777;
            }}
            .footer a {{
                color: #007BFF;
                text-decoration: none;
            }}
        </style>
        </head>
        <body>
            <div class='container'>
            <div class='header'>
                <h1>{Title}</h1>
            </div>
            <div class='content'>
                <p>Hello {email},</p>
                <p>{content}</p>
                <div class='otp-code'>{otpCode}</div>
                <p>This code will expire in 10 minutes. If you did not request this, please ignore this email.</p>
                <p>Best regards,<br/>Mind Map Generator Team</p>
            </div>
            <div class='footer'>
                <p>&copy; 2025 . All rights reserved.</p>
                <p><a href='#'>Privacy Policy</a> | <a href='#'>Contact Support</a></p>
            </div>
            </div>
         </body>
         </html>";
        }

    }
}
