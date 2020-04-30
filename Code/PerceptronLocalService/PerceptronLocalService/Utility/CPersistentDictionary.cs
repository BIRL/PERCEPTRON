using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PerceptronLocalService.Utility
{
    public class CPersistentDictionary
    {
        //! CPersistentDictionary class
        /*!
         * This class enables us to create a persistent dictionary i.e. Keys in memory while values on disk. It was taken from Code Project Website
         * 
         */

        Dictionary<string, string> _lstIndexes = new Dictionary<string, string>();
        private readonly FileStream _file;
        readonly string _completeFilename = @"C:\persist.dic";//index file will be, persist.hash.idx
        readonly string _indexFile;
        readonly int _arrayOffset = 0;//always zero

        public CPersistentDictionary(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            { _completeFilename = filename; }

            //The index file.
            //TODO: Merge the index file inside the array file.
            _indexFile = _completeFilename + ".idx";


            _file = new FileStream(_completeFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Init();//Load the indexes
        }

        public void Add(string key, string value)
        {
            //See if we already have a key,
            //If yes, this means, user wants to overwrite the
            //existing value
            if (_lstIndexes.ContainsKey(key))
            {
                //Retrieve the "key"'s item position
                //Update the value
                var pos = Convert.ToInt64(_lstIndexes[key].Split('|')[0]);
                var len = Convert.ToInt32(_lstIndexes[key].Split('|')[1]);

                //Set the position
                Int64 jumpTo = pos - 1 * len;//position - 1 (because of zero based index) * length

                //Jump to the position
                _file.Seek(jumpTo, SeekOrigin.Begin);
                _file.Write(Encoding.ASCII.GetBytes(value.ToCharArray()), _arrayOffset, value.Length);

                //Update the position of the index/length in the list
                _lstIndexes[key] = _file.Position + "|" + value.Length.ToString();
            }
            else
            {
                _file.Write(Encoding.ASCII.GetBytes(value.ToCharArray()), _arrayOffset, value.Length);
                _lstIndexes.Add(key, _file.Position + "|" + value.Length.ToString());

            }
        }

        public void Remove(string key)
        {
            //Requires logic
            if (!_lstIndexes.ContainsKey(key))
                return; //Maintain silence.

            //Replace something with nothing.
            string value = string.Empty;//"".PadLeft(_lstIndexes[key].Length, ' ');/

            //Retrieve the "key"'s item position
            //Update the value
            var pos = Convert.ToInt64(_lstIndexes[key].Split('|')[0]);
            var len = Convert.ToInt32(_lstIndexes[key].Split('|')[1]);

            //Set the position
            Int64 jumpTo = pos - 1 * len;//position - 1 (because of zero based index) * length

            //Jump to the position
            _file.Seek(jumpTo, SeekOrigin.Begin);

            //Dirty cleanup.
            //TODO: Required, GENUINE cleanup!
            _file.Write(Encoding.ASCII.GetBytes(value.ToCharArray()), _arrayOffset, value.Length);

            //Update the position of the index/length in the list
            _lstIndexes.Remove(key);


        }

        private string Read(string key)
        {
            string p = "";
            string l = "";
            try
            {
                if (!_lstIndexes.ContainsKey(key))
                    return string.Empty;//Keep silence.
                p = _lstIndexes[key].Split('|')[0];
                l = _lstIndexes[key].Split('|')[1];
                var pos = Convert.ToInt64(p);
                var len = Convert.ToInt32(l);

                //Set the position
                var jumpTo = pos - 1 * len;//position - 1 (because of zero based index) * length

                //Jump to the position
                _file.Seek(jumpTo, SeekOrigin.Begin);

                var bytesRead = new byte[len];

                //Read bytes
                _file.Read(bytesRead, 0, len);

                //Convert bytes into string
                var val = Encoding.UTF8.GetString(bytesRead);
                return val;
            }
            catch (Exception e11)
            {
                System.Diagnostics.Debug.WriteLine(e11.Message + "   " + _lstIndexes[key] + "  " + p + "  " + l);
                return "-1";
            }


        }

        private void Close()
        {
            if (_lstIndexes == null || _lstIndexes.Count == 0) return;

            using (FileStream fs = new FileStream(_indexFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(fs, _lstIndexes);
            }
        }

        private void Init()
        {
            using (FileStream fs = new FileStream(_indexFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                if (fs.Length < 1)
                    return;

                BinaryFormatter bin = new BinaryFormatter();
                _lstIndexes = bin.Deserialize(fs) as Dictionary<string, string>;

            }
        }

        //Emulate to be the of this file-hashtable
        public object this[string key] { get { return Read(key); } set { Add(key, value == null ? string.Empty : value.ToString()); } }

        //Count

        public int Count { get { return _lstIndexes.Count; } }

        public KeyValuePair<string, string> this[int index] { get { return _lstIndexes.ElementAt(index); } }

        #region IDisposable Members

        public void Dispose()
        {
            _file.Close();
            _file.Dispose();
            Close();
            _lstIndexes.Clear();
            _lstIndexes = null;

        }

        #endregion
    }
}
