using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System.Reflection;

using System.Text;
using System.Linq;

using Yue.Extensions;

namespace Yue.CommandLine
{
    /// <summary>
    /// Command Line class to handle custom method calls during Play Mode or builds.
    /// This class should come inside a prefab that you can drag into your hierarchy.
    /// </summary>
    public class CommandLine : MonoBehaviour
    {
        /// <summary>
        /// Keycode to char: https://gist.github.com/b-cancel/c516990b8b304d47188a7fa8be9a1ad9
        /// Recommended for any dictionary searches to be done once and cached
        /// </summary>
        Dictionary<char, KeyCode> chartoKeycode = new Dictionary<char, KeyCode>()
        {
          //-------------------------LOGICAL mappings-------------------------
  
          //Lower Case Letters
          {'a', KeyCode.A},
          {'b', KeyCode.B},
          {'c', KeyCode.C},
          {'d', KeyCode.D},
          {'e', KeyCode.E},
          {'f', KeyCode.F},
          {'g', KeyCode.G},
          {'h', KeyCode.H},
          {'i', KeyCode.I},
          {'j', KeyCode.J},
          {'k', KeyCode.K},
          {'l', KeyCode.L},
          {'m', KeyCode.M},
          {'n', KeyCode.N},
          {'o', KeyCode.O},
          {'p', KeyCode.P},
          {'q', KeyCode.Q},
          {'r', KeyCode.R},
          {'s', KeyCode.S},
          {'t', KeyCode.T},
          {'u', KeyCode.U},
          {'v', KeyCode.V},
          {'w', KeyCode.W},
          {'x', KeyCode.X},
          {'y', KeyCode.Y},
          {'z', KeyCode.Z},
  
          //KeyPad Numbers
          {'1', KeyCode.Keypad1},
          {'2', KeyCode.Keypad2},
          {'3', KeyCode.Keypad3},
          {'4', KeyCode.Keypad4},
          {'5', KeyCode.Keypad5},
          {'6', KeyCode.Keypad6},
          {'7', KeyCode.Keypad7},
          {'8', KeyCode.Keypad8},
          {'9', KeyCode.Keypad9},
          {'0', KeyCode.Keypad0},
  
          //Other Symbols
          {'!', KeyCode.Exclaim}, //1
          {'"', KeyCode.DoubleQuote},
          {'#', KeyCode.Hash}, //3
          {'$', KeyCode.Dollar}, //4
          {'&', KeyCode.Ampersand}, //7
          {'\'', KeyCode.Quote}, //remember the special forward slash rule... this isnt wrong
          {'(', KeyCode.LeftParen}, //9
          {')', KeyCode.RightParen}, //0
          {'*', KeyCode.Asterisk}, //8
          {'+', KeyCode.Plus},
          {',', KeyCode.Comma},
          {'-', KeyCode.Minus},
          {'.', KeyCode.Period},
          {'/', KeyCode.Slash},
          {':', KeyCode.Colon},
          {';', KeyCode.Semicolon},
          {'<', KeyCode.Less},
          {'=', KeyCode.Equals},
          {'>', KeyCode.Greater},
          {'?', KeyCode.Question},
          {'@', KeyCode.At}, //2
          {'[', KeyCode.LeftBracket},
          {'\\', KeyCode.Backslash}, //remember the special forward slash rule... this isnt wrong
          {']', KeyCode.RightBracket},
          {'^', KeyCode.Caret}, //6
          {'_', KeyCode.Underscore},
          {'`', KeyCode.BackQuote},
  
          //-------------------------NON-LOGICAL mappings-------------------------
  
          //NOTE: all of these can easily be remapped to something that perhaps you find more useful
  
          //---Mappings where the logical keycode was taken up by its counter part in either (the regular keybaord) or the (keypad)
  
          //Alpha Numbers
          //NOTE: we are using the UPPER CASE LETTERS Q -> P because they are nearest to the Alpha Numbers
          {'Q', KeyCode.Alpha1},
          {'W', KeyCode.Alpha2},
          {'E', KeyCode.Alpha3},
          {'R', KeyCode.Alpha4},
          {'T', KeyCode.Alpha5},
          {'Y', KeyCode.Alpha6},
          {'U', KeyCode.Alpha7},
          {'I', KeyCode.Alpha8},
          {'O', KeyCode.Alpha9},
          {'P', KeyCode.Alpha0},
  
          //INACTIVE since I am using these characters else where
          {'A', KeyCode.KeypadPeriod},
          {'B', KeyCode.KeypadDivide},
          {'C', KeyCode.KeypadMultiply},
          {'D', KeyCode.KeypadMinus},
          {'F', KeyCode.KeypadPlus},
          {'G', KeyCode.KeypadEquals},
  
          //-------------------------CHARACTER KEYS with NO KEYCODE-------------------------
  
          //NOTE: you can map these to any of the OPEN KEYCODES below
  
          /*
          //Upper Case Letters (16)
          {'H', -},
          {'J', -},
          {'K', -},
          {'L', -},
          {'M', -},
          {'N', -},
          {'S', -},
          {'V', -},
          {'X', -},
          {'Z', -}
          */
  
          //-------------------------KEYCODES with NO CHARACER KEY-------------------------
  
          //-----KeyCodes without Logical Mappings
          //-Anything above "KeyCode.Space" in Unity's Documentation (9 KeyCodes)
          //-Anything between "KeyCode.UpArrow" and "KeyCode.F15" in Unity's Documentation (24 KeyCodes)
          //-Anything Below "KeyCode.Numlock" in Unity's Documentation [(28 KeyCodes) + (9 * 20 = 180 JoyStickCodes) = 208 KeyCodes]
  
          //-------------------------other-------------------------

          //-----KeyCodes that are inaccesible for some reason
          //{'~', KeyCode.tilde},
          //{'{', KeyCode.LeftCurlyBrace}, 
          //{'}', KeyCode.RightCurlyBrace}, 
          //{'|', KeyCode.Line},   
          //{'%', KeyCode.percent},
        };

        private struct Parameter
        {
            public string parameterType;
            public string parameterName;

            public Parameter(string parameterType, string parameterName)
            {
                this.parameterType = parameterType;
                this.parameterName = parameterName;
            }

            public string ParameterName()
            {
                return parameterType + " " + parameterName;
            }
        }
        private struct Function
        {
            public string functionName;
            public Parameter[] functionParameters;

            public Function(string functionName, Parameter[] functionParameters)
            {
                this.functionName = functionName;
                this.functionParameters = functionParameters;
            }

            public string GetParametersInFormat()
            {
                if (functionParameters.Length <= 0)
                    return "";

                List<string> parameters = new List<string>();
                foreach (Parameter p in functionParameters)
                    parameters.Add(p.parameterType + " " + p.parameterName);

                return "(" + string.Join(",", parameters.ToArray()) + ")";
            }
        }
        /// <summary>
        /// The class that contains the function names of a single monobehaviour attached.
        /// </summary>
        private class CommandMonoBehaviour
        {
            public MonoBehaviour monoBehaviour;
            public List<Function> functions = new List<Function>();

            public void AddFunction(string functionName, Parameter[] functionParameters)
            {
                functions.Add(new Function(functionName, functionParameters));
            }
        }
        
        /// <summary>
        /// A class that contains all monobehaviours attached that want custom function calls to be enabled.
        /// </summary>
        [Serializable]
        private class CommandLineObject
        {
            /// <summary>
            /// The object with the monobehaviours attached.
            /// </summary>
            public GameObject commandObject;

            /// <summary>
            /// List of all monobehaviours attached to the "commandObject"
            /// </summary>
            [HideInInspector] public List<CommandMonoBehaviour> commandTypes = new List<CommandMonoBehaviour>();
            
            /// <summary>
            /// Name of this object
            /// </summary>
            [HideInInspector] public string commandObjectName;

            /// <summary>
            /// Whether this object requires attributes to work.
            /// Enabling this means that all functions that want to be called from this command line need to be attributed with [Command]
            /// </summary>
            public bool requireAttribute;

            /// <summary>
            /// Initialisation. This function needs to be called once at the start. (Either in Awake or Start)
            /// </summary>
            public void Init()
            {
                // Set the object's name
                commandObjectName = commandObject.name;

                // Get all monobehaviours from the specified "commandObject"
                MonoBehaviour[] mbObjects = commandObject.GetComponentsInChildren<MonoBehaviour>();
                
                // Add them to the list
                foreach (MonoBehaviour mb in mbObjects)
                {
                    // Create new and add...
                    CommandMonoBehaviour cmb = new CommandMonoBehaviour();
                    cmb.monoBehaviour = mb;
                    commandTypes.Add(cmb);
                }

                // Loop through all monobehaviours
                foreach (CommandMonoBehaviour mb in commandTypes)
                {
                    //Type mbType = mb.monoBehaviour.GetType();

                    // Get all the function members in this monobehaviour
                    MethodInfo[] methodInfo = mb.monoBehaviour.GetType().GetMethods();

                    foreach (MethodInfo mi in methodInfo)
                    {
                        if (requireAttribute)
                        {
                            // If the attribute is required for all methods, check for the attribute
                            Attribute attr = mi.GetCustomAttribute(typeof(Command), true);
                            if (attr != null && attr.GetType() == typeof(Command))
                            {
                                List<Parameter> parameters = new List<Parameter>();
                                ParameterInfo[] parameterInfo = mi.GetParameters();
                                foreach (ParameterInfo pi in parameterInfo)
                                    parameters.Add(new Parameter(pi.ParameterType.Name.ToString(), pi.Name));
                                mb.AddFunction(mi.Name, parameters.ToArray());
                                //mb.functionNames.Add(mi.Name); // Add the method only if the attribute is valid
                            }
                        }
                        else
                        {
                            // If else no attribute is needed, just add the method in.
                            //mb.functionNames.Add(mi.Name);
                            List<Parameter> parameters = new List<Parameter>();
                            ParameterInfo[] parameterInfo = mi.GetParameters();
                            foreach (ParameterInfo pi in parameterInfo)
                                parameters.Add(new Parameter(pi.ParameterType.Name.ToString(), pi.Name));

                            mb.AddFunction(mi.Name, parameters.ToArray());
                        }
                    }

                    // Remove all ignored commands (e.g. GameObject methods, MonoBehaviour methods)
                    RemoveIgnoreCommands(mb.functions);
                }
            }

            /// <summary>
            /// Removes GameObject and Monobehaviour functions.
            /// Since GameObject functions such as GameObject.Find() and Monobehaviour functions such as Monobehaviour.Invoke()
            /// are for developmental purposes, we don't want the user to be able to access these.
            /// So we need to remove and disable them.
            /// </summary>
            /// <param name="functions"></param>
            private void RemoveIgnoreCommands(List<Function> functions)
            {
                Type gameObjectType = typeof(GameObject);
                MemberInfo[] goMemberInfo = gameObjectType.GetMembers();
                foreach (MemberInfo mi in goMemberInfo)
                    functions.RemoveAll(s => s.functionName == mi.Name);

                Type mbType = typeof(MonoBehaviour);
                MemberInfo[] mbMemberInfo = mbType.GetMembers();
                foreach (MemberInfo mi in mbMemberInfo)
                    functions.RemoveAll(s => s.functionName == mi.Name);
            }
        }

        /// <summary>
        /// The key to bring up the GUI of this command line
        /// </summary>
        [Header("Functionality")]
        [SerializeField] private KeyCode commandKey = KeyCode.Slash;
        [SerializeField] private KeyCode predictionCycleUp = KeyCode.UpArrow;
        [SerializeField] private KeyCode predictionCycleDown = KeyCode.DownArrow;

        /// <summary>
        /// Corresponding char of the commandKey
        /// </summary>
        private char commandChar;

        /// <summary>
        /// List of all the objects that are allowed to be called by the command line
        /// </summary>
        [SerializeField] private List<CommandLineObject> commandLineObjects = new List<CommandLineObject>();

        [SerializeField] private List<string> commandLineObjectsDontDestroyOnLoad = new List<string>();
        #region GUI_VARIABLES
        public enum ColorTheme
        {
            Light,
            Dark,
        }

        /// <summary>
        /// Canvas...?
        /// </summary>
        private Canvas GUICanvas;
        /// <summary>
        /// Input field where the command is entered and checked
        /// </summary>
        private TMP_InputField commandInputField;
        /// <summary>
        /// string to show commands ran and error messages in OnGUI
        /// </summary>
        private string cmdLog = "";
        /// <summary>
        /// Max length of cmdLog
        /// </summary>
        private int maxCharLen = 700;

        private int previousPredictionCount = 0;

        private ContentFitter contentFitter;

        [Header("GUI Customisation")]
        /// <summary>
        /// Variable to control visibility of OnGUI Unity event
        /// </summary>
        [SerializeField] private bool showGUI = false;
        [SerializeField] private ColorTheme colorTheme = ColorTheme.Dark;
        #endregion

        /// <summary>
        /// Prediction button pool to store and show predicted commands above inputfield
        /// </summary>
        private CommandPredictionButtonPool predictionButtonPool;

        private void Awake()
        {
            foreach (string dd in commandLineObjectsDontDestroyOnLoad)
            {
                GameObject obj = GameObject.Find(dd);
                if (obj != null)
                {
                    CommandLineObject clObj = new CommandLineObject();
                    clObj.commandObject = obj;
                    clObj.requireAttribute = true;
                    commandLineObjects.Add(clObj);
                }
                else
                    Debug.LogError("Unable to find objects " + dd + " from DontDestroyOnLoad objects." +
                        "Be sure to check your spelling or make sure code order is correct.");
            }

            foreach (CommandLineObject obj in commandLineObjects)
                obj.Init();

            GUICanvas = GetComponentInChildren<Canvas>();
            predictionButtonPool = GetComponentInChildren<CommandPredictionButtonPool>();
            contentFitter = GetComponentInChildren<ContentFitter>();

            GUICanvas.gameObject.SetActive(showGUI);
            commandInputField = GUICanvas.GetComponentInChildren<TMP_InputField>();
            commandChar = chartoKeycode.FirstOrDefault(x => x.Value == commandKey).Key;
        }

        /// <summary>
        /// Sets the text in the inputfield for command
        /// </summary>
        /// <param name="command"></param>
        public void SetInputfield(string command)
        {
            commandInputField.text = command;
        }

        /// <summary>
        /// Predicts the command by looking through all stored commands
        /// </summary>
        /// <param name="commandEntered">
        /// Command requested to predict
        /// </param>
        public void PredictCommand(string commandEntered)
        {
            if (commandEntered.Length <= 0)
            {
                predictionButtonPool.ReturnObjects();
                contentFitter.Resize();
                previousPredictionCount = 0;
                return;
            }

            //string command = FilterCommand(commandEntered);
            List<Tuple<string, string>> predicts = new List<Tuple<string, string>>();
            foreach (CommandLineObject obj in commandLineObjects)
            {
                foreach (CommandMonoBehaviour mb in obj.commandTypes)
                {
                    foreach (Function func in mb.functions)
                    {
                        string s = func.functionName;
                        string combined = string.Format("{0}.{1}.{2}", obj.commandObjectName, mb.monoBehaviour.GetType(), s);
                        if (combined.Contains(commandEntered, StringComparison.OrdinalIgnoreCase))
                            predicts.Add(new Tuple<string, string>(combined, combined + func.GetParametersInFormat()));
                        else if (commandEntered.Contains(combined, StringComparison.OrdinalIgnoreCase))
                            predicts.Add(new Tuple<string, string>(combined, combined + func.GetParametersInFormat()));
                    }
                }
            }

            int predictionCount = 0;
            if (predicts.Count > 0)
            {
                predictionCount = predictionButtonPool.CreatePredictionButtons(commandEntered, predicts.ToArray()).Count;
                predictionButtonPool.ResetSelection();
            }

            if (predictionCount != previousPredictionCount)
                contentFitter.Resize();

            previousPredictionCount = predictionCount;
        }


        public void ClosePredictions()
        {
            predictionButtonPool.ReturnObjects();
            contentFitter.Resize();
        }

        /// <summary>
        /// Prepares to call a function w/o parameters
        /// </summary>
        /// <param name="commandEntered">
        /// Command that is requested
        /// </param>
        public void CallCommand(string commandEntered)
        {
            StringBuilder objectName = new StringBuilder("");
            StringBuilder monoBehaviourName = new StringBuilder("");
            StringBuilder functionName = new StringBuilder("");
            //string command = FilterCommand(commandEntered);
            string[] parametersArray = null;
            int index = 0;

            //try
            {
                if (commandEntered.Length > 0)
                {
                    for (int i = 0; i < commandEntered.Length; ++i)
                    {
                        while (i < commandEntered.Length && commandEntered[i] != '.')
                        {
                            if (char.IsWhiteSpace(commandEntered[i]))
                                goto BreakNested;
                            switch (index)
                            {
                                case 0:
                                    objectName.Append(commandEntered[i]);
                                    break;
                                case 1:
                                    monoBehaviourName.Append(commandEntered[i]);
                                    break;
                                case 2:
                                    functionName.Append(commandEntered[i]);
                                    break;
                            }
                            ++i;
                        }

                        ++index;
                    }

                BreakNested:

                    StringBuilder parameters = new StringBuilder("");
                    int indexToCut = 0;
                    for (int i = 0; i < commandEntered.Length; ++i)
                    {
                        if (char.IsWhiteSpace(commandEntered[i]))
                            indexToCut = ++i;
                    }

                    if (indexToCut != 0)
                    {
                        for (int i = indexToCut; i < commandEntered.Length; ++i)
                        {
                            parameters.Append(commandEntered[i]);
                        }
                    }

                    string par = parameters.ToString();
                    if (parameters.Length > 0 && !string.IsNullOrEmpty(par))
                        parametersArray = par.Split(',');
                }

                CallFunction(objectName.ToString(), monoBehaviourName.ToString(), functionName.ToString(), parametersArray);
            }
            //catch (Exception e)
            //{
            //    throw e;
            //}
        }

        /// <summary>
        /// Calls a function that is valid from commandLineObjects
        /// </summary>
        /// <param name="objectName">
        /// The name of the object.
        /// </param>
        /// <param name="monoBehaviourName">
        /// The name of the MonoBehaviour attached.
        /// </param>
        /// <param name="functionName">
        /// The name of the function in the Monobehaviour attached.
        /// </param>
        /// <param name="parameters">
        /// Any parameters needed in function call.
        /// </param>
        private void CallFunction(string objectName, string monoBehaviourName, string functionName, object[] parameters = null)
        {
            foreach (CommandLineObject obj in commandLineObjects)
            {
                foreach (CommandMonoBehaviour mb in obj.commandTypes)
                {
                    if (mb.monoBehaviour.GetType().ToString().CompareTo(monoBehaviourName) == 0 &&
                        obj.commandObjectName.CompareTo(objectName) == 0)
                    {
                        if (mb.functions.Where(f => f.functionName == functionName).Count() > 0)
                        {
                            Type type = mb.monoBehaviour.GetType();
                            MethodInfo method = type.GetMethod(functionName);
                            object returned = method.Invoke(mb.monoBehaviour, parameters);
                            string returnString = returned == null ? "" : "returned with " + returned.ToString(); 
                            Log(RecreateFunction(method, parameters)
                                + returnString);
                        }
                        else
                        {
                            Debug.LogError("Unable to find specified command/function to call "
                                + string.Format("{0}.{1}.{2}", objectName, monoBehaviourName, functionName));

                            Log("Unable to find specified command/function to call "
                                + string.Format("{0}.{1}.{2}", objectName, monoBehaviourName, RecreateFunction(functionName, parameters)));
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Filters a command of its commandKey
        /// </summary>
        /// <param name="command">
        /// Command requested
        /// </param>
        /// <returns>
        /// Command without commandKey. E.g. "/wait" becomes "wait"
        /// </returns>
        private string FilterCommand(string command)
        {
            if (command.Length > 0 && command[0].CompareTo(commandChar) == 0)
            {
                StringBuilder sb = new StringBuilder(command);
                sb.Remove(0, 1);

                return sb.ToString();
            }
            return command;
        }

        private void Update()
        {
            if (Input.GetKeyDown(commandKey))
            {
                bool show = !GUICanvas.gameObject.activeSelf;
                GUICanvas.gameObject.SetActive(show);
                showGUI = show;
            }

            if (Input.GetKeyDown(predictionCycleUp))
                predictionButtonPool.SelectPrevious();
            if (Input.GetKeyDown(predictionCycleDown))
                predictionButtonPool.SelectNext();
        }

        #region GUI
        private void InitialiseTheme()
        {
            switch (colorTheme)
            {
                case ColorTheme.Light:
                    break;
                case ColorTheme.Dark:
                    break;
            }
        }

        private void OnGUI()
        {
            if (!showGUI) return;
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
                new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
            GUI.TextArea(new Rect(10, 10, 540, 370), cmdLog);
        }

        private string RecreateFunction(string methodName, object[] parameters)
        {
            string param = "";
            if (parameters != null && parameters.Length > 0)
                param = string.Join(",", parameters);

            return methodName + "(" + param + ");";
        }
        private string RecreateFunction(MethodInfo method, object[] parameters)
        {
            string param = "";
            if (parameters != null && parameters.Length > 0)
                param = string.Join(",", parameters);

            return method.Name + "(" + param + ");";
        }

        public void Log(string logString)
        {
            cmdLog = cmdLog + "\n" + logString;
            if (cmdLog.Length > maxCharLen) { cmdLog = cmdLog.Substring(cmdLog.Length - maxCharLen); }
        }
        #endregion

    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Command : Attribute { }
}