using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create a clean texture mask - a green texture with black mask - use this dirtclean texture to mask against and create a clean effect!!
/// </summary>
public class SN_ObjectsDirtCleaner
{
    private Texture2D dirtCleanMask;

    public SN_ObjectsDirtCleaner(Texture2D dirtCleanMask)
    {
        this.dirtCleanMask = dirtCleanMask;
    }

    /// <summary>
    /// Clean Pixels with RayCast Hit point - in a circular shape!!
    /// </summary>
    /// <param name="radius"> Radius of Circle</param>
    /// <param name="hit">RayCast hit point</param>
    public void CleanPixels_ShapeCircle(float radius, RaycastHit hit)
    {
        int hitPixelX = (int)(hit.textureCoord.x * dirtCleanMask.width);
        int hitPixelY = (int)(hit.textureCoord.y * dirtCleanMask.height);

        CleanPixelsWithCircleShape(radius, hitPixelX, hitPixelY);
    }

    /// <summary>
    /// Clean Pixels with RayCast Hit point - in a circular shape!!
    /// </summary>
    /// <param name="radius"> Radius of Circle</param>
    /// <param name="effectCenterX">EffectCenterX - on to Clean Texture - where circle should start from</param>
    /// <param name="effectCenterY">EffectCenterY - on to Clean Texture - where circle should start from</param>
    public void CleanPixels_ShapeCircle(float radius, int effectCenterX, int effectCenterY)
    {
        CleanPixelsWithCircleShape(radius, effectCenterX, effectCenterY);
    }

    private void CleanPixelsWithCircleShape(float radius, int effectCenterX, int effectCenterY)
    {
        for (int i = 0; i < radius; i++)
        {
            for (int j = 0; j < radius; j++)
            {
                if (Vector2.Distance(new Vector2(effectCenterX, effectCenterY), new Vector2(effectCenterX + i, effectCenterY + j)) < radius)
                {
                    dirtCleanMask.SetPixel(effectCenterX + i, effectCenterY + j, new Color(0, 0, 0, 0));
                    dirtCleanMask.SetPixel(effectCenterX - i, effectCenterY + j, new Color(0, 0, 0, 0));
                    dirtCleanMask.SetPixel(effectCenterX - i, effectCenterY - j, new Color(0, 0, 0, 0));
                    dirtCleanMask.SetPixel(effectCenterX + i, effectCenterY - j, new Color(0, 0, 0, 0));
                }
            }
        }

        dirtCleanMask.Apply();
    }

    /// <summary>
    /// Calculates the percentage cleaned
    /// </summary>
    /// <returns></returns>
    public float PercentageCleaned()
    {
        Color[] pixels = dirtCleanMask.GetPixels();  //NOTE!! - This GetPixel() is the culprit method, which reduces the performance.
        int totalPixels = pixels.Length;

        int totalBlackPixels = 0; // Cleaned
        Color black = new Color(0, 0, 0, 0); // Transparent black

        foreach (Color pixel in pixels)
        {
            if (pixel == black)
            {
                totalBlackPixels += 1;
            }
        }

        return ((float)totalBlackPixels / totalPixels) * 100f;
    }

    public void ResetDirtMaskTexture()
    {
        for (int i = 0; i < dirtCleanMask.width; i++)
        {
            for (int j = 0; j < dirtCleanMask.height; j++)
            {
                dirtCleanMask.SetPixel(i, j, new Color(0, 1, 0));
            }
        }

        dirtCleanMask.Apply();
    }

    public void AutoFullClean()
    {
        Color[] clearColors = new Color[dirtCleanMask.width * dirtCleanMask.height];

        // Initialize the array with transparent colors
        for (int i = 0; i < clearColors.Length; i++)
        {
            clearColors[i] = new Color(0, 0, 0, 0);
        }

        // Set all pixels at once
        dirtCleanMask.SetPixels(clearColors);
        dirtCleanMask.Apply();
    }
}
