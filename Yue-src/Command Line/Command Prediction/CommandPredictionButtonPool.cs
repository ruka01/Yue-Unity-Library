using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Yue.DesignPatterns;
namespace Yue.CommandLine
{
    public class CommandPredictionButtonPool : ObjectPool<CommandPredictionButton>
    {
        [SerializeField] private Transform creationParent;
        private List<CommandPredictionButton> enabledButtons = new List<CommandPredictionButton>();
        [SerializeField] private CommandLine commandLine;

        private int selectedIndex = 0;

        private new void Awake()
        {
            base.Awake();
        }

        protected override void CreateObjects(int count)
        {
            for (int i = 0; i < count; ++i)
            {
                GameObject newObject = Instantiate(objectToPool);
                newObject.SetActive(enableObjects);
                newObject.transform.SetParent(transform);
                CommandPredictionButton button = newObject.GetComponent<CommandPredictionButton>();
                button.Init(commandLine);
                pool.Add(button);
            }
        }
        public List<CommandPredictionButton> CreatePredictionButtons(string commandEntered, Tuple<string,string>[] predictions)
        {
            ReturnObjects();

            foreach (Tuple<string,string> s in predictions)
            {
                CommandPredictionButton button = Get();
                button.transform.SetParent(creationParent);
                button.SetCommand(commandEntered, s.Item1, s.Item2);
                button.Deselect();
                enabledButtons.Add(button);
            }

            return enabledButtons;
        }
        public void ReturnObjects()
        {
            enabledButtons.ForEach(f => Return(f));
            enabledButtons.Clear();
        }
        public override void Return(CommandPredictionButton obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);
        }

        public void ResetSelection()
        {
            if (enabledButtons.Count < 0)
                return;

            if (selectedIndex < enabledButtons.Count)
                enabledButtons[selectedIndex].Deselect();

            selectedIndex = 0;
            enabledButtons[selectedIndex].Select();
        }

        public void SelectPrevious()
        {
            if (enabledButtons.Count <= 0)
                return;

            enabledButtons[selectedIndex].Deselect();

            selectedIndex -= 1;
            if (selectedIndex < 0)
                selectedIndex = enabledButtons.Count - 1;

            enabledButtons[selectedIndex].Select();

            Debug.Log("Enabled Buttons Count: " + enabledButtons.Count);
        }

        public void SelectNext()
        {
            if (enabledButtons.Count <= 0)
                return;

            enabledButtons[selectedIndex].Deselect();
            selectedIndex = (selectedIndex + 1) % enabledButtons.Count;
            enabledButtons[selectedIndex].Select();

            Debug.Log("Enabled Buttons Count: " + enabledButtons.Count);
        }

        private void Select()
        {
            enabledButtons[selectedIndex].OnButtonClicked();
        }
    }
}