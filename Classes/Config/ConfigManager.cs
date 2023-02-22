using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WinCFScan.Classes.HTTPRequest;

namespace WinCFScan.Classes.Config
{
    internal class ConfigManager
    {
        protected string v2rayTemplateConfigFileName = "v2ray-config/config.json.template";
        public string? v2rayConfigTemplate { get; private set; }
        protected string[] mandatoryDirectories = { "v2ray-config", "v2ray-config/generated", "results" }; //this dirs must be existed

        protected AppConfig? appConfig;
        protected RealConfig? realConfig;

        public static ConfigManager? Instance { get; private set; }

        public ConfigManager()
        {
            if (this.load() && appConfig != null)
            {
                realConfig = new RealConfig(appConfig);
            }

            // set static instance for later access of this instance
            Instance = this;
        }

        protected bool load()
        {
            try
            {
                // create mandatory directories
                foreach (var dir in mandatoryDirectories)
                {
                    if (!Directory.Exists(dir));
                    {
                        Directory.CreateDirectory(dir);
                    }
                }

                // app config
                appConfig = new AppConfig();
                appConfig.load(); // this must be called
                appConfig = appConfig.getLoadedInstance();

                // load v2ray config template
                if (!File.Exists(v2rayTemplateConfigFileName))
                {
                    return false;
                }

                v2rayConfigTemplate = File.ReadAllText(v2rayTemplateConfigFileName);

            }
            catch(Exception ex)
            {
                return false;
            }

            return isConfigValid();
        }

        public bool isConfigValid()
        {
            if(appConfig != null && appConfig.isConfigValid() && v2rayConfigTemplate != null)
            {
                return true;
            }

            return false;
        }


        public AppConfig? getAppConfig()
        {
            return this.appConfig;
        }

        public RealConfig? getRealConfig()
        {
            return this.realConfig;
        }
    }
}
