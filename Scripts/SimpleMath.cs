using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_SimpleMath
{
    // INFO : "float"형 전용 제곱 함수입니다.
    // "float"형이 아닌 수에 대한 예외는 지원하지 않습니다.
    public static float Square(float fNumber)
    {
        return fNumber * fNumber;
    }

    public static float GetSpeed(ref float fHorizontal, ref float fVertical)
    {
        return Square(fHorizontal) + Square(fVertical);
    }
}
