using UnityEngine;
using UnityEngine.EventSystems;
namespace XWEngine.UGUI
{
    public class EventPass : MonoBehaviour,
        IPointerClickHandler,
        ISubmitHandler,
        IMoveHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        ISelectHandler,
        IDeselectHandler
    {
        public GameObject passGo;

        //监听点击
        public void OnPointerClick(PointerEventData eventData)
        {
            if (passGo != null && passGo.TryGetComponent<IPointerClickHandler>(out var result)) {
                result.OnPointerClick(eventData);
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            if (passGo != null && passGo.TryGetComponent<ISubmitHandler>(out var result)) {
                result.OnSubmit(eventData);
            }
        }
        public void OnMove(AxisEventData eventData)
        {
            if (passGo != null && passGo.TryGetComponent<IMoveHandler>(out var result)) {
                result.OnMove(eventData);
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (passGo != null && passGo.TryGetComponent<IPointerDownHandler>(out var result)) {
                result.OnPointerDown(eventData);
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (passGo != null && passGo.TryGetComponent<IPointerUpHandler>(out var result)) {
                result.OnPointerUp(eventData);
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (passGo != null && passGo.TryGetComponent<IPointerEnterHandler>(out var result)) {
                result.OnPointerEnter(eventData);
            }
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (passGo != null && passGo.TryGetComponent<IPointerExitHandler>(out var result)) {
                result.OnPointerExit(eventData);
            }
        }
        public void OnSelect(BaseEventData eventData)
        {
            if (passGo != null && passGo.TryGetComponent<ISelectHandler>(out var result)) {
                result.OnSelect(eventData);
            }
        }
        public void OnDeselect(BaseEventData eventData)
        {
            if (passGo != null && passGo.TryGetComponent<IDeselectHandler>(out var result)) {
                result.OnDeselect(eventData);
            }
        }
    }
}