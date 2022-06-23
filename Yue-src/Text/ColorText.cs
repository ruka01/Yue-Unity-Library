using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Yue.Extensions;

namespace Yue.Text
{
    /// <summary>
    /// Color Text class. Extension for Unity's customisation for coloring text.
    /// </summary>
    public static class ColorText
    {
        /// <summary>
        /// Coloring of text for TextMeshPro.
        /// </summary>
        /// <param name="text">
        /// The entire text you want to color.
        /// </param>
        /// <param name="highlight">
        /// The part of the text where you want to color.
        /// </param>
        /// <param name="color">
        /// Color of the highlight using UnityEngine's struct "Color"
        /// </param>
        /// <returns>
        /// string of text where Unity's TextMeshProUGUI can color code the specified highlight text
        /// </returns>
        public static string TMPro(string text, string highlight, Color color)
        {
            if (text.Length <= 0 || highlight.Length <= 0)
                return text;

            string hex = ColorUtility.ToHtmlStringRGB(color);
            StringBuilder sb = new StringBuilder("");

            int hIndex = 0;
            int startIndex = 0;

            for (int i = 0; i < text.Length; ++i)
            {
                if (text[i].CompareTo(highlight[hIndex], StringComparison.OrdinalIgnoreCase))
                {
                    sb.Append(text[i]);
                    if (sb.ToString().CompareTo(highlight, StringComparison.OrdinalIgnoreCase))
                    {
                        startIndex = i - hIndex;
                        break;
                    }
                    ++hIndex;
                }
                else
                {
                    i -= hIndex;
                    i = Mathf.Clamp(i, 0, text.Length);
                    hIndex = 0;
                    sb.Clear();
                }
            }

            StringBuilder finalSb = new StringBuilder(text);
            string removed = finalSb.ToString(startIndex, sb.Length);
            finalSb.Remove(startIndex, sb.Length);

            StringBuilder hlText = new StringBuilder("");
            hlText.Append("<color=#");
            hlText.Append(hex);
            hlText.Append(">");
            hlText.Append(removed);
            hlText.Append("</color>");

            finalSb.Insert(startIndex, hlText.ToString());

            return finalSb.ToString();
        }
    }
}