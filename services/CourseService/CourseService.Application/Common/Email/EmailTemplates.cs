namespace CourseService.Application.Common.Email;

public static class EmailTemplates
{
    public static class AddResultsToPracticalItemSubmit
    {
        public static string Subject => "Перевірено виконане завдання на платформі Seminarium";

        public static string GetTemplate(string teacherName, string topic, string link, string? text)
        {
            return $@"
        <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f3f4f6; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
            <h1 style='color: #333; text-align: center; font-size: 1.5rem; margin-bottom: 20px;'>
                <b>{teacherName} перевірили ваше виконане практичне завдання</b> <b>{topic}</b> на платформі Seminarium.
            </h1>
            <p style='font-family: Arial, sans-serif; color: #666; margin-bottom: 20px;'>
                {text}
            </p>
            <div style='text-align: center; margin-bottom: 20px;'>
                <a href='{link}' style='display: inline-block; padding: 10px 20px; font-family: Arial, sans-serif; font-size: 16px; color: #fff; background-color: #28a745; border-radius: 5px; text-decoration: none;'>
                    Переглянути результати
                </a>
            </div>
        </div>";
        }
    }
}
