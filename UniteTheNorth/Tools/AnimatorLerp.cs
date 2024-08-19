using UnityEngine;

namespace UniteTheNorth.Tools;

public class AnimatorFloatLerp
{
    private readonly Animator _animator;
    private readonly Dictionary<int, float> _currentDict = new();
    private readonly Dictionary<int, float> _goalDict = new();

    public AnimatorFloatLerp(Animator animator)
    {
        _animator = animator;
    }

    public void Update()
    {
        foreach (var id in _goalDict.Keys)
        {
            _currentDict[id] = Mathf.Lerp(_currentDict[id], _goalDict[id], 2 * Time.deltaTime);
            _animator.SetFloat(id, _currentDict[id]);
            if (Mathf.Abs(_currentDict[id] - _goalDict[id]) < .01F)
                _goalDict.Remove(id);
        }
    }
    
    public void SetFloat(int id, float val)
    {
        if (!_currentDict.ContainsKey(id))
            _currentDict[id] = _animator.GetFloat(id);
        _goalDict[id] = val;
    }
}