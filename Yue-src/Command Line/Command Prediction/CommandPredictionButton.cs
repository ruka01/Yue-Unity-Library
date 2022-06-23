using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Yue.Text;
using UnityEngine.UI;

namespace Yue.CommandLine
{
    public class CommandPredictionButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI commandText;
        private string command;
        private string comamndToDisplay;
        private Image background;
        public Color regularColor, highlightColor;

        private CommandLine commandLine;
        private void Awake()
        {
            background = GetComponent<Image>();
            Deselect();
        }
        public void Init(CommandLine commandLine)
        {
            this.commandLine = commandLine;
        }
        
        public void SetCommand(string command)
        {
            this.command = command;
            commandText.text = command;
        }

        public void SetCommand(string commandEntered, string originalCommand, string commandToDisplay)
        {
            command = originalCommand;
            commandText.text = ColorText.TMPro(commandToDisplay, commandEntered, Color.red);
        }

        public void OnButtonClicked()
        {
            commandLine.SetInputfield(command);
            commandLine.ClosePredictions();
        }

        public void Select()
        {
            background.color = highlightColor;
        }
        public void Deselect()
        {
            background.color = regularColor;
        }
    }
}