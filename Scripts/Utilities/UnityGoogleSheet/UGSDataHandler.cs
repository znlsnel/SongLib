#if SET_UGS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleSheet;
using UGS;


public class UGSDataHandler<T> where T : ITable
{
    protected Dictionary<int, T> datas = new Dictionary<int, T>();
    protected List<T> dataList = new List<T>();

    public UGSDataHandler()
    {
        dataList = UnityGoogleSheet.GetList<T>();
        datas = UnityGoogleSheet.GetDictionary<int, T>();
    

        Init();
    }

    protected virtual void Init()
    {
        
    }

    
    public List<T> GetAll()
    {
        return dataList;
    }

    public T GetByIndex(int index)
    {
        if (datas.TryGetValue(index, out T data))
            return data;

        return default(T);
    }

    public T GetByCondition(Func<T, bool> condition)
    {
        foreach (T data in dataList)
        {
            if (condition(data))
                return data;
        }

        return default(T);;
    }

    public List<T> GetAllByCondition(Func<T, bool> condition)
    {
        List<T> items = new List<T>();
        foreach (T data in dataList)
        {
            if (condition(data))
                items.Add(data);
        }

        return items;
    }

    public int GetCount() => dataList.Count;
}
#endif