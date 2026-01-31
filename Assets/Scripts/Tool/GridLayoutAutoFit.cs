using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 让 Grid Layout Group 的 Cell Size 和 Spacing 自适应屏幕分辨率
/// </summary>
[RequireComponent(typeof(GridLayoutGroup))]
public class GridLayoutAutoFit : MonoBehaviour
{
    [Header("参考分辨率（设计稿分辨率）")]
    [Tooltip("通常是你的UI设计稿分辨率，比如1920x1080")]
    public Vector2 referenceResolution = new Vector2(1920, 1080);

    [Header("基础单元格尺寸（参考分辨率下）")]
    [Tooltip("在参考分辨率下，单元格的宽高")]
    public Vector2 baseCellSize = new Vector2(100, 100);

    [Header("基础间距（参考分辨率下）")]
    [Tooltip("在参考分辨率下，单元格之间的间距")]
    public Vector2 baseSpacing = new Vector2(20, 20);

    [Header("适配方向")]
    [Tooltip("优先按宽度适配/优先按高度适配/按最小比例适配")]
    public FitMode fitMode = FitMode.Min;

    private GridLayoutGroup _gridLayout;

    // 适配模式枚举
    public enum FitMode
    {
        Width,   // 优先按宽度比例适配
        Height,  // 优先按高度比例适配
        Min      // 按宽高比例中较小的值适配（防止超出屏幕）
    }

    private void Awake()
    {
        // 获取Grid Layout Group组件
        _gridLayout = GetComponent<GridLayoutGroup>();
        // 初始化适配
        UpdateGridLayout();
    }

    private void OnRectTransformDimensionsChange()
    {
        // 当UI区域尺寸变化时（比如屏幕分辨率改变），重新适配
        UpdateGridLayout();
    }

    /// <summary>
    /// 计算并更新单元格尺寸和间距
    /// </summary>
    private void UpdateGridLayout()
    {
        if (_gridLayout == null) return;

        // 获取当前屏幕的实际分辨率（UI的有效分辨率）
        float currentWidth = Screen.width;
        float currentHeight = Screen.height;

        // 计算宽高的缩放比例
        float scaleX = currentWidth / referenceResolution.x;
        float scaleY = currentHeight / referenceResolution.y;

        // 根据适配模式选择最终缩放比例
        float finalScale = 1f;
        switch (fitMode)
        {
            case FitMode.Width:
                finalScale = scaleX;
                break;
            case FitMode.Height:
                finalScale = scaleY;
                break;
            case FitMode.Min:
                finalScale = Mathf.Min(scaleX, scaleY);
                break;
        }

        // 1. 计算适配后的单元格尺寸
        Vector2 newCellSize = new Vector2(
            baseCellSize.x * finalScale,
            baseCellSize.y * finalScale
        );

        // 2. 计算适配后的间距
        Vector2 newSpacing = new Vector2(
            baseSpacing.x * finalScale,
            baseSpacing.y * finalScale
        );

        // 应用新的尺寸和间距
        _gridLayout.cellSize = newCellSize;
        _gridLayout.spacing = newSpacing;
    }

    // 编辑器下实时预览（可选）
    private void OnValidate()
    {
        if (Application.isPlaying && _gridLayout != null)
        {
            UpdateGridLayout();
        }
    }
}