using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZLC.WindowSystem.Attribute;
namespace XWEngine.UGUI
{
    /// <summary>
    /// 加载进度条：文本+进度
    /// </summary>
    [ShortKey]
    public class XWLoading : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _text;

        public void Refresh(int finished, int total)
        {
            _slider.maxValue = total;
            _slider.value = finished;
            _text.text = $"<color=#00FFFF>加载中...</color>{finished}/{total}";
        }
        
        public void Refresh(float progress)
        {
            _slider.maxValue = 1;
            _slider.value = progress;
            _text.text = $"<color=#00FFFF>加载中...</color>{progress * 100}%";
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if(_slider == null) _slider = GetComponent<Slider>();
            if (_text == null) _text = GetComponentInChildren<TextMeshProUGUI>();
        }
        #endif
    }
}