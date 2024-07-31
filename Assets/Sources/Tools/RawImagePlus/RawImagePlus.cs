using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// RawImage扩展 
/// 图片比例为原始比例
/// </summary>
public class RawImagePlus : RawImage
{
    public ScaleMode scaleMode=ScaleMode.ScaleToFit;
    public ScaleMode ScaleMode
    {
        get { return scaleMode; }
        set
        {
            scaleMode = value;
            SetVerticesDirty();
        }
    }

    private List<UIVertex> _vertices = new List<UIVertex>(4);
    private static List<int> QuadIndices = new List<int>(new int[] { 0, 1, 2, 2, 3, 0 });

    //这个函数只会发生在网格重建中调用
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        _OnFillVBO(_vertices);

        vh.AddUIVertexStream(_vertices, QuadIndices);
    }

    /// <summary>
    /// 填充顶点数据
    /// </summary>
    /// <param name="vbo"></param>
    private void _OnFillVBO(List<UIVertex> vbo)
    {
        Rect uvRect = this.uvRect;
        Vector4 v = GetDrawingDimensions(ScaleMode, ref uvRect);

        vbo.Clear();

        var vert = UIVertex.simpleVert;
        vert.color = color;

        vert.position = new Vector2(v.x, v.y);
        vert.uv0 = new Vector2(uvRect.xMin, uvRect.yMin);
        vbo.Add(vert);


        vert.position = new Vector2(v.x, v.w);
        vert.uv0 = new Vector2(uvRect.xMin, uvRect.yMax);
        vbo.Add(vert);

        vert.position = new Vector2(v.z, v.w);
        vert.uv0 = new Vector2(uvRect.xMax, uvRect.yMax);
        vbo.Add(vert);

        vert.position = new Vector2(v.z, v.y);
        vert.uv0 = new Vector2(uvRect.xMax, uvRect.yMin);
        vbo.Add(vert);
    }

    /// <summary>
    /// 获取绘制区域
    /// </summary>
    /// <param name="scaleMode"></param>
    /// <param name="uvRect"></param>
    /// <returns></returns>
    private Vector4 GetDrawingDimensions(ScaleMode scaleMode, ref Rect uvRect)
    {
        Vector4 returnSize = Vector4.zero;

        if (mainTexture != null)
        {
            var padding = Vector4.zero;



            Rect r = GetPixelAdjustedRect();

            // Fit the above textureSize into rectangle r
            Vector2 textureSize = new Vector2(mainTexture.width, mainTexture.height);
            int spriteW = Mathf.RoundToInt(textureSize.x);
            int spriteH = Mathf.RoundToInt(textureSize.y);

            var size = new Vector4(padding.x / spriteW,
                                    padding.y / spriteH,
                                    (spriteW - padding.z) / spriteW,
                                    (spriteH - padding.w) / spriteH);

            {
                if (textureSize.sqrMagnitude > 0.0f)
                {
                    if (scaleMode == ScaleMode.ScaleToFit)
                    {
                        float spriteRatio = textureSize.x / textureSize.y;
                        float rectRatio = r.width / r.height;

                        if (spriteRatio > rectRatio)
                        {
                            float oldHeight = r.height;
                            r.height = r.width * (1.0f / spriteRatio);
                            r.y += (oldHeight - r.height) * rectTransform.pivot.y;
                        }
                        else
                        {
                            float oldWidth = r.width;
                            r.width = r.height * spriteRatio;
                            r.x += (oldWidth - r.width) * rectTransform.pivot.x;
                        }
                    }
                    else if (scaleMode == ScaleMode.ScaleAndCrop)
                    {
                        float aspectRatio = textureSize.x / textureSize.y;
                        float screenRatio = r.width / r.height;
                        if (screenRatio > aspectRatio)
                        {
                            float adjust = aspectRatio / screenRatio;
                            uvRect = new Rect(uvRect.xMin, (uvRect.yMin * adjust) + (1f - adjust) * 0.5f, uvRect.width, adjust * uvRect.height);
                        }
                        else
                        {
                            float adjust = screenRatio / aspectRatio;
                            uvRect = new Rect(uvRect.xMin * adjust + (0.5f - adjust * 0.5f), uvRect.yMin, adjust * uvRect.width, uvRect.height);
                        }
                    }
                }
            }

            returnSize = new Vector4(r.x + r.width * size.x,
                                      r.y + r.height * size.y,
                                      r.x + r.width * size.z,
                                      r.y + r.height * size.w);
        }

        return returnSize;
    }
}
