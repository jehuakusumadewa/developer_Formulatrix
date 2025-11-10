



var stack = new Stack<int>();
stack.Push(5);
stack.Push(10);
int x = stack.Pop();
int y = stack.Pop();
System.Console.WriteLine($"{x}, {y}");

public class Stack<T>
{
    int position;
    T[] data = new T[100];

    public void Push(T obj) => data[position++] = obj;
    public T Pop() => data[--position];

}