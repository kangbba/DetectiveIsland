using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Manager<T> where T : ScriptableObject
{
    protected List<T> _dataList;
    protected ArokaAnim _mainPanel;

    public List<T> DataList => _dataList;
    public ArokaAnim MainPanel => _mainPanel;

     public virtual void Initialize(string folderName, ArokaAnim mainPanel)
    {
        LoadDatas(folderName);
        _mainPanel = mainPanel;
    }

    private void LoadDatas(string folderName)
    {
        _dataList = new List<T>();

        // 폴더 내의 모든 ScriptableObject를 가져와 리스트에 추가
        string folderPath = "Assets/Resources/" + folderName;
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        FileInfo[] files = directoryInfo.GetFiles("*.asset");

        foreach (FileInfo file in files)
        {
            string path = "Assets/Resources/" + folderName + "/" + file.Name.Replace(".asset", "");
            T data = Resources.Load<T>(path);
            if (data != null)
            {
                _dataList.Add(data);
            }
        }
    }

    public void SetOnPanel(bool isActive, float totalTime)
    {
        if (_mainPanel != null)
        {
            _mainPanel.SetAnim(isActive, totalTime);
        }
    }
}
