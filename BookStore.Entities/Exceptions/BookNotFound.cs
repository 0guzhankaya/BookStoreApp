namespace BookStore.Entities.Exceptions
{
    // sealed is final version
    public sealed class BookNotFoundException : NotFoundException
    {
        public BookNotFoundException(int id) : base($"The book with id: {id} could not found!")
        {
        }
    }
}
