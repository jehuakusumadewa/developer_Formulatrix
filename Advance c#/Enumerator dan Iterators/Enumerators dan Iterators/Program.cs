public interface IEnumerable
{
    IEnumerator GetEnumerator();
}

public interface IEnumerator
{
    bool MoveNext();
    object Current { get; }
    void Reset();
}
public class NumberCollection : IEnumerable
{
    private int[] numbers;

    public NumberCollection(int[] nums)
    {
        numbers = nums;
    }

    public IEnumerator GetEnumerator()
    {
        return new NumberEnumerator(numbers);

}
    public class NumberEnumerator : IEnumerator
    {
        private int[] numbers;
        private int position = -1;
        public NumberEnumerator(int[] nums)
        {
            numbers = nums;
        }
        public bool MoveNext()
        {
            position++;
            return position < numbers.Length;
        }
        public object Current
        {
            get
            {
                if (position < 0 || position >= numbers.Length)
                    throw new InvalidOperationException();
                return numbers[position];
            }
        }
         public void Reset()
        {
                position = -1;
        }
        
        

    }
    
}