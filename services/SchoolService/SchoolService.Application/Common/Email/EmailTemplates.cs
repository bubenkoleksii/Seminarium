namespace SchoolService.Application.Common.Email;

public static class EmailTemplates
{
    public static class CreateJoiningRequest
    {
        public static string Subject => "Успішне створення запиту на приєднання навчального закладу на платформі Seminarium";

        public static string GetTemplate(Guid id, string name)
        {
            return $@"
                <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f3f4f6; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
                    <h1 style='color: #333; text-align: center; font-size: 1.5rem; margin-bottom: 20px;'>Прийнято запит на приєднання навчального закладу {name} на приєднання до платформи Seminarium.</h1>
                    <p style='font-family: Arial, sans-serif; color: #666; margin-bottom: 20px;'>Очікуйте повідомлення з відповіддю на цю електронну скриньку або за вказаним номером телефону.</p>
                    <p style='font-family: Arial, sans-serif; color: #666; margin-bottom: 20px;'><strong>Ідентифікатор запиту:</strong> {id}</p>
                </div>";
        }
    }

    public static class RejectJoiningRequest
    {
        public static string Subject => "Відхилено запит на приєднання навчального закладу на платформі Seminarium";

        public static string GetTemplate(Guid id, string name, string? text)
        {
            return $@"
                <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f3f4f6; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
                    <h1 style='color: #333; text-align: center; font-size: 1.5rem; margin-bottom: 20px;'><b>Відхилено</b> запит на приєднання навчального закладу {name} на приєднання до платформи Seminarium.</h1>
                    <p style='font-family: Arial, sans-serif; color: #666; margin-bottom: 20px;'>{text}</p>
                    <p style='font-family: Arial, sans-serif; color: #666; margin-bottom: 20px;'>Якщо маєте якісь запитання, надсилайте у відповідь на цей лист</p>
                    <p style='font-family: Arial, sans-serif; color: #666; margin-bottom: 20px;'><strong>Ідентифікатор запиту:</strong> {id}</p>
                </div>";
        }
    }
}
