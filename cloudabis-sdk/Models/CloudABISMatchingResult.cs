using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace cloudabis_sdk.Models
{
    /// <summary>
    /// CloudABIS Matching Result
    /// </summary>
    public class CloudABISResponse
    {
        /// <summary>
        /// Customer Key that was used to make the request.
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// Name of operation that was requested.
        /// </summary>
        public EnumOperationName OperationName { get { return this._operationName; } set { this._operationName = value; } }
        /// <summary>
        /// The operation execution status.
        /// </summary>
        public EnumOperationStatus Status
        {
            get { return this._opStatus; }
            set { this._opStatus = value; }
        }
        /// <summary>
        /// The result of the operation.
        /// </summary>
        public string OperationResult { get; set; }
        /// <summary>
        /// Biometric matched record with the highest confidence score.
        /// </summary>
        public ScoreResult BestResult
        {
            get
            {

                if (this._results.Count > 0)
                {
                    this._results = this._results.OrderByDescending(r => r.Score).ToList();
                    return this._results[0];
                }
                else
                    return new ScoreResult();
            }
            set { }
        }

        /// <summary>
        /// All biometric matched records.
        /// </summary>
        public List<ScoreResult> DetailResult { get { return this._results; } }


        /// <summary>
        /// Entity of matched record, containing Member ID and associated matching score.
        /// </summary>
        public class ScoreResult
        {
            /// <summary>
            /// 
            /// </summary>
            public ScoreResult()
            {
                this.Score = 0;
                this.ID = string.Empty;
            }
            /// <summary>
            /// Strength of biometric match.
            /// </summary>
            public int Score;
            /// <summary>
            /// Member ID of matched record.
            /// </summary>
            public string ID;
            /// <summary>
            /// Matched finger position
            /// </summary>
            public int FingerPosition;
        }

        List<ScoreResult> _results = new List<ScoreResult>();
        EnumOperationName _operationName = EnumOperationName.None;
        EnumOperationStatus _opStatus = EnumOperationStatus.NONE;

        /// <summary>
        /// 
        /// </summary>
        public CloudABISResponse() { }

        /// <summary>
        /// Number of biometric matched records.
        /// </summary>
        public int MatchCount
        {
            get { return this._results.Count; }
            set { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(); ns.Add("", "");
            XmlWriterSettings settings = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = true };
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CloudABISResponse));
                serializer.Serialize(writer, this, ns);
                return sb.ToString();
            }
        }
    }
}
