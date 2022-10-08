
using XTC.FMP.LIB.MVCS;
using System.Xml;
using System.Xml.Serialization;

namespace XTC.FMP.MOD.Vendor.LIB.MVCS
{
    /// <summary>
    /// Unity数据层
    /// </summary>
    public class UnityModel : UnityModelBase
    {
        public class DependencyConfig
        {
            public class Field
            {
                [XmlAttribute("attribute")]
                public string attribute { get; set; } = "";

                [XmlAttribute("values")]
                public string values { get; set; } = "";
            }


            public class Options
            {
                [XmlAttribute("environment")]
                public string environment { get; set; } = "develop";
                [XmlAttribute("repository")]
                public string repository { get; set; } = "";
            }

            public class Reference
            {
                [XmlAttribute("org")]
                public string org { get; set; } = "";
                [XmlAttribute("module")]
                public string module { get; set; } = "";
                [XmlAttribute("version")]
                public string version { get; set; } = "";
            }

            public class Plugin
            {
                [XmlAttribute("name")]
                public string name { get; set; } = "";
                [XmlAttribute("version")]
                public string version { get; set; } = "";
            }

            public class Body
            {
                [XmlElement("Options")]
                public Options options = new Options();
                [XmlArray("References"), XmlArrayItem("Reference")]
                public Reference[] references { get; set; } = new Reference[0];
                [XmlArray("Plugins"), XmlArrayItem("Plugin")]
                public Plugin[] plugins { get; set; } = new Plugin[0];
            }

            public class Schema
            {
                [XmlElement("Body")]
                public Body body { get; set; } = new Body();

                [XmlArray("Header"), XmlArrayItem("Field")]
                public Field[] fields { get; set; } = new Field[]
                {
                    new Field {
                        attribute = "LogLevel.environment",
                        values = "环境，可选值为：develop,product。使用develop时，所有依赖的version字段被强制替换为develop",
                    }
                };
            }

            [XmlElement("Schema")]
            public Schema schema { get; set; } = new Schema();
        }

        public class BootloaderConfig
        {
            public class BootStep
            {
                [XmlAttribute("length")]
                public int length = 1;
                [XmlAttribute("org")]
                public string org = "";
                [XmlAttribute("module")]
                public string module = "";
            }

            public class Schema
            {
                [XmlArray("Steps"), XmlArrayItem("Step")]
                public BootStep[] steps { get; set; } = new BootStep[0];
            }

            [XmlElement("Schema")]
            public Schema schema { get; set; } = new Schema();
        }

        public class UpgradeConfig
        {
            public class Schema
            {
                public class Field
                {
                    [XmlAttribute("attribute")]
                    public string attribute { get; set; } = "";

                    [XmlAttribute("values")]
                    public string values { get; set; } = "";
                }


                public class Update
                {
                    [XmlAttribute("strategy")]
                    public string strategy { get; set; } = "skip";
                }

                public class Body
                {
                    [XmlElement("Update")]
                    public Update update { get; set; } = new Update();
                }

                [XmlElement("Body")]
                public Body body { get; set; } = new Body();

                [XmlArray("Header"), XmlArrayItem("Field")]
                public Field[] fields { get; set; } = new Field[] {
                    new Field {
                        attribute = "Update.strategy",
                        values = "升级策略，可选值为：skip, auto, manual",
                    },
                };
            }

            [XmlElement("Schema")]
            public Schema schema { get; set; } = new Schema();
        }

        /// <summary>
        /// 完整名称
        /// </summary>
        public const string NAME = "XTC.FMP.MOD.Vendor.LIB.MVCS.UnityModel";

        /// <summary>
        /// Unity状态
        /// </summary>
        public class UnityStatus : Model.Status
        {
        }

        /// <summary>
        /// 带uid参数的构造函数
        /// </summary>
        /// <param name="_uid">实例化后的唯一识别码</param>
        /// <param name="_gid">直系的组的ID</param>
        public UnityModel(string _uid, string _gid) : base(_uid, _gid)
        {
        }

        protected override void preSetup()
        {
            base.preSetup();

            // 实例化直系状态
            Error err;
            status_ = spawnStatus<UnityStatus>(this.getUID() + ".Status", out err);
            if (0 != err.getCode())
            {
                getLogger()?.Error(err.getMessage());
            }
            else
            {
                getLogger()?.Trace("setup {0}", this.getUID() + ".Status");
            }
        }

        protected override void preDismantle()
        {
            base.preDismantle();

            // 销毁直系状态
            Error err;
            killStatus(this.getUID() + ".Status", out err);
            if (0 != err.getCode())
            {
                getLogger()?.Error(err.getMessage());
            }
        }

    }
}


