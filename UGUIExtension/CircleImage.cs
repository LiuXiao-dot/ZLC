using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;
namespace XWEngine.UGUI
{

    public class CircleImage : Image
    {
        [SerializeField] private float showPercent = 1f;
        [SerializeField] private int segments = 100;
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            float width = rectTransform.rect.width;
            float height = rectTransform.rect.height;
            Vector2 uv = GetUV(overrideSprite);
            Vector2 convertRatio = GetConvetRatio(uv.x, uv.y, width, height);
            Vector2 uvCenter = GetUvCenter(uv.x, uv.y);
            Vector2 originPos = GetOriginPos(width, height);
            UIVertex origin = new UIVertex();
            //设置原点颜色渐变
            byte c = (byte)(showPercent * 255);
            origin.color = new Color32(c, c, c, 255);
            origin.position = originPos;
            origin.uv0 = new Vector2(Vector2.zero.x * convertRatio.x + uvCenter.x, Vector2.zero.y * convertRatio.y + uvCenter.y);
            vh.AddVert(origin);

            float radian = Mathf.PI * 2 / segments;
            float curRadian = 0;
            float radius = width * 0.5f;
            int realSegment = (int)(showPercent * segments);
            for (int i = 0; i < segments + 1; i++) {
                float x = Mathf.Cos(curRadian) * radius;
                float y = Mathf.Sin(curRadian) * radius;
                curRadian += radian;
                Vector2 xy = new Vector2(x, y);
                UIVertex uvTemp = new UIVertex();
                if (i < realSegment) {
                    uvTemp.color = color;
                } else {
                    if (realSegment == segments) {
                        uvTemp.color = color;
                    } else {
                        uvTemp.color = new Color32(60, 60, 60, 255);
                    }
                }
                uvTemp.position = xy + originPos;
                uvTemp.uv0 = new Vector2(xy.x * convertRatio.x + uvCenter.x, xy.y * convertRatio.y + uvCenter.y);
                vh.AddVert(uvTemp);
            }

            int id = 1;
            for (int i = 0; i < segments; i++) {
                vh.AddTriangle(id, 0, id + 1);
                id++;
            }

        }
        private Vector2 GetConvetRatio(float uvWidth, float uvHeight, float width, float height)
        {

            Vector2 convertRatio = new Vector2(uvWidth / width, uvHeight / height);
            return convertRatio;
        }

        private Vector2 GetUvCenter(float uvWidth, float uvHeight)
        {
            Vector2 center = new Vector2(uvWidth * 0.5f, uvHeight * 0.5f);
            return center;
        }

        private Vector2 GetOriginPos(float width, float height)
        {
            Vector2 originPos = new Vector2((0.5f - rectTransform.pivot.x) * width, ((0.5f - rectTransform.pivot.y) * height));
            return originPos;
        }
        private Vector2 GetUV(Sprite sprite)
        {
            Vector4 temp = sprite != null ? DataUtility.GetOuterUV(sprite) : Vector4.zero;
            float uvHeight = temp.w - temp.y;
            float uvWidth = temp.z - temp.x;
            Vector2 uv = new Vector2(uvWidth, uvHeight);
            return uv;
        }

    }
}