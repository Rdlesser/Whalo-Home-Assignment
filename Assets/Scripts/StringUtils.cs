using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class StringUtils {

    private static readonly int _thousand = 1000;
    private static readonly int _tenThousand = 10000;
    private static readonly int _million = 1000000;
    private static readonly int _tenMillion = 10000000;
    private static readonly int _billion = 1000000000;
    private static readonly long _tenBillion = 10000000000;
    private static readonly long _trillion = 1000000000000;
    private static readonly long _tenTrillion = 10000000000000;
        
    public static IEnumerator RollNumber(TMP_Text text, long start, long end, float time, bool shortString = true,
        string prefix = null, string suffix = null, float startDelay = 0, Slider slider = null) {

        var delta = end - start;
        var startTime = Time.time + startDelay;
        var endTime = startTime + time;

        while(Time.time < startTime) {
            yield return null;
        }

        while(Time.time < endTime) {
            double timeT = (Time.time - startTime) / time;
            var val = start + (timeT * delta);
            text.text = prefix + NumberToString(Convert.ToInt64(val), shortString) + suffix;
            if(slider != null) {
                slider.value = (int)val;
            }
            yield return null;
        }

        text.text = prefix + NumberToString(end, shortString) + suffix;
    }
        
    public static string NumberToString(long value, bool shortString = false) {

        if(shortString)
            return NumberToShortString(value);
        else
            return NumberToLongString(value);
    }
        
    public static string NumberToShortString(long num, int pad = -1) {

        long abs = System.Math.Abs(num);
        string retVal;

        if(abs < _thousand) {
            retVal = abs.ToString();
        } else if(abs < _tenThousand) {
            double x = (double)abs / _thousand;
            if((x % 1) == 0) {
                retVal = x + "K";
            } else {
                retVal = x.ToString("#.#") + "K";
            }
        } else if(abs < _million) {
            retVal = (abs / _thousand).ToString() + "K";
        } else if(abs < _tenMillion) {
            double x = (double)abs / _million;
            if((x % 1) == 0) {
                retVal = x + "M";
            } else {
                retVal = x.ToString("#.#") + "M";
            }
        } else if(abs < _billion) {
            retVal = (abs / _million).ToString() + "M";
        } else if(abs < _tenBillion) {
            double x = (double)abs / _billion;
            if((x % 1) == 0) {
                retVal = x + "B";
            } else {
                retVal = x.ToString("#.#") + "B";
            }
        } else if(abs < _trillion) {
            retVal = (abs / _billion).ToString() + "B";
        } else if(abs < _tenTrillion) {
            double x = (double)abs / _trillion;
            if((x % 1) == 0) {
                retVal = x + "T";
            } else {
                retVal = x.ToString("#.#") + "T";
            }
        } else {
            retVal = (abs / _trillion).ToString() + "T";
        }

        retVal = (num < 0 ? "-" : "") + retVal;
        return Pad(retVal, pad, ' ');
    }
        
    public static string NumberToLongString(long value) {

        var builder = new StringBuilder();
        builder.Append(value);

        for(int i = builder.Length - 3; i > 0; i -= 3) {
            builder.Insert(i, ",");
        }

        return builder.ToString();
    }
        
    public static string Pad(string str, int totalLength, char padding) {

        if((totalLength > 0) && (totalLength > str.Length)) {
            totalLength = totalLength - str.Length;
            var half = totalLength / 2;
            str = new string(padding, half) + str;
            totalLength -= half;
            str += new string(padding, totalLength);
        }
        return str;
    }

}