namespace Homework2.Models
{
    //Для повідомлень

    //повдомлення
    //- ід
    //- від кого (адреса)
    //- текст повідомлення
    //- дата час
    public class Message
    {
        private static int nextId = 1;
        public int Id { get; set; } = nextId++;
        public string Address { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
