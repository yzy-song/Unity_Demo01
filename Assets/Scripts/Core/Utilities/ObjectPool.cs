using System;
using System.Collections.Generic;

public class ObjectPool<T> where T : class
{
    private readonly Stack<T> stack = new Stack<T>();
    private readonly Func<T> createFunc;

    public ObjectPool(Func<T> createFunc, int initialSize = 0)
    {
        this.createFunc = createFunc;

        // 初始化对象池
        for (int i = 0; i < initialSize; i++)
        {
            stack.Push(createFunc());
        }
    }

    public T Get()
    {
        return stack.Count > 0 ? stack.Pop() : createFunc();
    }

    public void Release(T obj)
    {
        stack.Push(obj);
    }
}
