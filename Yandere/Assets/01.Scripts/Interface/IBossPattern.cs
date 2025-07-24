using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossPattern
{
    bool IsDone { get; }            // 현재 패턴이 완료되었는지
    bool CanExecute();              // 현재 조건에서 실행 가능한지
    void Execute();                 // 패턴 실행 시작
}
