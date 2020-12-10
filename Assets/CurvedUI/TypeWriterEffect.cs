using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class TypeWriterEffect : MonoBehaviour
{
    //변경할 변수
    public float delay;
    public float Skip_delay;
    public int cnt;
    string[] fulltext;
    public int dialog_cnt;
    string currentText;

    //타이핑확인 변수
    public bool text_exit;
    public bool text_full;
    public bool text_cut;


    public SubElement[] explainArrayStep;
    public TextMeshProUGUI text;


    public ShowingImageMovement showingImageMovement;
    private void Awake()
    {
        
    }



    public void StartTypingIndex(int index)
    {
        PopUpMessage(()=>
        {
            Typing(index);
        }
        );
     
    }
    void Typing(int index)
    {
        text.text = "";
        if (explainArrayStep[index].onStartEvent != null) explainArrayStep[index].onStartEvent.Invoke();
        Get_TypingIndex(explainArrayStep[index].explains.Length, explainArrayStep[index].explains, index);
    }


    //
    //
    //
    // //텍스트 시작호출
    public void Get_TypingIndex(int _dialog_cnt, string[] _fullText,int index)
    {
        //재사용을 위한 변수초기화
        text_exit = false;
        text_full = false;
        text_cut = false;
        cnt = 0;

        //변수 불러오기
        dialog_cnt = _dialog_cnt;
        fulltext = new string[dialog_cnt];
        fulltext = _fullText;

        //타이핑 코루틴시작
        StartCoroutine(ShowTextIndex(fulltext, index));
    }

    IEnumerator ShowTextIndex(string[] _fullText, int index)
    {
        if(showingImageMovement!=null)
        {
            showingImageMovement.SetImage(""+index + cnt);
        }
      

        if (cnt >= dialog_cnt)
        {
            text_exit = true;
            Debug.Log(index + "/" + "모든텍스트 종료");
            if (explainArrayStep[index].onFinishEvent != null)
            {
                Debug.Log("onFinishEvent");
                explainArrayStep[index].onFinishEvent.Invoke();
            }


            StopCoroutine("showText");


        }
        else
        {

            //기존문구clear
            currentText = "";
           // Speak(_fullText[cnt]);
            //타이핑 시작
            for (int i = 0; i < _fullText[cnt].Length; i++)
            {
                //타이핑중도탈출
                if (text_cut == true)
                {
                    break;
                }
                text.text = "";
                //단어하나씩출력
                currentText = _fullText[cnt].Substring(0, i + 1);
                string[] texts = currentText.Split('|');
                for (int j = 0; j < texts.Length; j++)
                {
                    if(j< texts.Length-1)
                    {
                        text.text += texts[j] + "\n";
                    }
                    else
                    {
                        text.text += texts[j];
                    }
                }
            
                yield return new WaitForSeconds(delay);
            }
            //탈출시 모든 문자출력
            Debug.Log("Typing 종료");
            text.text = _fullText[cnt];

          //  Debug.Log(Speaker.isSpeaking);
          //  yield return StartCoroutine(CheckEndOfSpeak());
          //  Debug.Log(Speaker.isSpeaking);
            yield return new WaitForSeconds(Skip_delay);
            if (showingImageMovement != null)
            {
                showingImageMovement.OffBoard();
            }
            //스킵_지연후 종료
            Debug.Log("Enter 대기");
            text_full = true;
            End_TypingIndex(index);
        }
    }


    public string voiceName;
   public void Speak(string words)
   {
       Debug.Log("tt");
      
   }

   // IEnumerator CheckEndOfSpeak()
   // {
   //     while (true)
   //     {
   //         yield return null;
   //         if(!Speaker.isSpeaking)
   //         {
   //             break;
   //         }
   //     }
   // }


    //다음버튼함수
    public void End_TypingIndex( int index)
    {
        //다음 텍스트 호출
        if (text_full == true)
        {
            cnt++;
            text_full = false;
            text_cut = false;
            StartCoroutine(ShowTextIndex(fulltext, index));
        }
        //텍스트 타이핑 생략
        else
        {
            text_cut = true;

        }
    }
    private void OnDisable()
    {
        text.text = "";
        transform.localScale = Vector3.zero;
    }
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
    }
    public void PopUpMessage(UnityAction callBack =null)
    {
        gameObject.SetActive(true);
        transform.DOScale(Vector3.one,1).SetEase(Ease.OutBack).OnComplete(callBack.Invoke);
    }
    

}
