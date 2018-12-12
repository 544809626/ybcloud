using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensitivewordApi.Helper.internals;

namespace SensitivewordApi.Helper.SensitiveWordHelper
{
    public class StringSearch : BaseSearch
    {
        public string FindFirst(string text)
        {
            TrieNode ptr = null;
            for (int i = 0; i < text.Length; i++)
            {
                TrieNode tn;
                if (ptr == null)
                {
                    tn = _first[text[i]];
                }
                else
                {
                    if (ptr.TryGetValue(text[i], out tn) == false)
                    {
                        tn = _first[text[i]];
                    }
                }
                if (tn != null)
                {
                    if (tn.End)
                    {
                        return tn.Results[0];
                    }
                }
                ptr = tn;
            }
            return null;
        }

        public List<string> FindAll(string text)
        {
            TrieNode ptr = null;
            List<string> list = new List<string>();

            for (int i = 0; i < text.Length; i++)
            {
                TrieNode tn;
                if (ptr == null)
                {
                    tn = _first[text[i]];
                }
                else
                {
                    if (ptr.TryGetValue(text[i], out tn) == false)
                    {
                        tn = _first[text[i]];
                    }
                }
                if (tn != null)
                {
                    if (tn.End)
                    {
                        foreach (var item in tn.Results)
                        {
                            list.Add(item);
                        }
                    }
                }
                ptr = tn;
            }
            return list;
        }

        public bool ContainsAny(string text)
        {
            TrieNode ptr = null;
            for (int i = 0; i < text.Length; i++)
            {
                TrieNode tn;
                if (ptr == null)
                {
                    tn = _first[text[i]];
                }
                else
                {
                    if (ptr.TryGetValue(text[i], out tn) == false)
                    {
                        tn = _first[text[i]];
                    }
                }
                if (tn != null)
                {
                    if (tn.End)
                    {
                        return true;
                    }
                }
                ptr = tn;
            }
            return false;
        }


        /// <summary>
        /// 替换方法
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="replaceChar">替换字符</param>
        /// <returns></returns>
        public string Replace(string text, char replaceChar = '*')
        {
            StringBuilder result = new StringBuilder(text);

            TrieNode ptr = null;
            for (int i = 0; i < text.Length; i++)
            {
                //if (!isCHS(text[i]) && !isNum(text[i]) && !isAlphabet(text[i]))
                //{
                //    continue;
                //} 
                TrieNode tn;
                if (ptr == null)
                {
                    tn = _first[text[i]];
                }
                else
                {
                    if (ptr.TryGetValue(text[i], out tn) == false)
                    {
                        tn = _first[text[i]];
                    }
                }
                if (tn != null)
                {

                    if (tn.End)
                    {
                        illegalWords.Add(tn.Results[0]);
                        Status = false;
                        var MaxLength = 0;
                        for (int j = 0; j < tn.Results.Count; j++)
                        {
                            if (tn.Results[j].Length > MaxLength)
                            {
                                MaxLength = tn.Results[j].Length;
                            }
                        }
                        var start = i + 1 - MaxLength;
                        for (int j = start; j <= i; j++)
                        {
                            result[j] = replaceChar;
                        }
                    }
                }
                ptr = tn;
            }
            return result.ToString();
        }

        /// <summary>
        /// 判断是否是中文
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        private bool isCHS(char character)
        {
            //  中文表意字符的范围 4E00-9FA5
            int charVal = (int)character;
            return (charVal >= 0x4e00 && charVal <= 0x9fa5);
        }

        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        private bool isNum(char character)
        {
            int charVal = (int)character;
            return (charVal >= 48 && charVal <= 57);
        }


        /// <summary>
        /// 判断是否是字母
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        private bool isAlphabet(char character)
        {
            int charVal = (int)character;
            return ((charVal >= 97 && charVal <= 122) || (charVal >= 65 && charVal <= 90));
        }

        #region 初始化值
        private List<string> illegalWords = new List<string>();
        /// <summary>
        /// 检测到的非法词集
        /// </summary>
        public List<string> IllegalWords
        {
            get { return illegalWords; }
        }

        /// <summary>
        /// 检测到的非法词集 
        /// </summary>
        private List<dynamic> hasIllegalWords = new List<dynamic>();
        public List<dynamic> HasIllegalWords
        {
            get { return hasIllegalWords; }
        }

        private bool status = true;
        /// <summary>
        /// 是否包含敏感词
        /// </summary>
        public bool Status
        {
            get { return status; }
            set { status = value; }
        }
        #endregion

    }
}
