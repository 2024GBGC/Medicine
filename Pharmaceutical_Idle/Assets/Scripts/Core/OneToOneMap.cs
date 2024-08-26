using System.Collections.Generic;

public class OneToOneMap<TKey, TValue>
{
    private Dictionary<TKey, TValue> keyToValue = new Dictionary<TKey, TValue>();
    private Dictionary<TValue, TKey> valueToKey = new Dictionary<TValue, TKey>();

    // 키와 값을 추가하는 메서드
    public bool Add(TKey key, TValue value)
    {
        // 키 또는 값이 이미 존재하는 경우 추가 실패
        if (keyToValue.ContainsKey(key) || valueToKey.ContainsKey(value))
        {
            return false;
        }

        keyToValue[key] = value;
        valueToKey[value] = key;
        return true;
    }

    // 키로 값을 가져오는 메서드
    public bool TryGetValueByKey(TKey key, out TValue value)
    {
        return keyToValue.TryGetValue(key, out value);
    }

    // 값으로 키를 가져오는 메서드
    public bool TryGetKeyByValue(TValue value, out TKey key)
    {
        return valueToKey.TryGetValue(value, out key);
    }

    // 키로 항목을 삭제하는 메서드
    public bool RemoveByKey(TKey key)
    {
        if (keyToValue.TryGetValue(key, out TValue value))
        {
            keyToValue.Remove(key);
            valueToKey.Remove(value);
            return true;
        }
        return false;
    }

    // 값으로 항목을 삭제하는 메서드
    public bool RemoveByValue(TValue value)
    {
        if (valueToKey.TryGetValue(value, out TKey key))
        {
            valueToKey.Remove(value);
            keyToValue.Remove(key);
            return true;
        }
        return false;
    }
    
    // 모든 키를 반환하는 메서드
    public IEnumerable<TKey> GetAllKeys()
    {
        return keyToValue.Keys;
    }

    // 모든 값을 반환하는 메서드
    public IEnumerable<TValue> GetAllValues()
    {
        return keyToValue.Values;
    }
}