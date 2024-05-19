namespace BaoXia.Utils.Models
{
        public class IPAddressInfo
        {
                ////////////////////////////////////////////////
                // @静态常量
                ////////////////////////////////////////////////

                #region 静态常量

                public const string FuzzyMatchingKey = "*";

                #endregion



                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                protected string? _ipString;

                public string? IPString
                {
                        get
                        {
                                return _ipString;
                        }
                        set
                        {
                                _ipString = value;
                                _ipSectionStrings = _ipString?.Split(".");
                        }
                }


                protected string[]? _ipSectionStrings;

                protected string[]? IPSectionStrings
                {
                        get
                        {
                                return _ipSectionStrings;
                        }
                        set
                        {
                                _ipSectionStrings = value;
                        }
                }

                public int SectionsCount
                {
                        get
                        {
                                if (_ipSectionStrings != null)
                                {
                                        return _ipSectionStrings.Length;
                                }
                                return 0;
                        }
                }

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public IPAddressInfo()
                { }

                public IPAddressInfo(string? ipString)
                {
                        this.IPString = ipString;
                }

                #endregion



                ////////////////////////////////////////////////
                // @重载
                ////////////////////////////////////////////////

                #region 重载

                public override bool Equals(object? obj)
                {
                        if (obj == null)
                        {
                                return false;
                        }

                        if (_ipSectionStrings?.Length == 1)
                        {
                                if (_ipSectionStrings[0].Equals(IPAddressInfo.FuzzyMatchingKey))
                                {
                                        // !!!
                                        return true;
                                        // !!!
                                }
                        }

                        IPAddressInfo? anotherIPAddressInfo = null;
                        if (obj is IPAddressInfo)
                        {
                                anotherIPAddressInfo = obj as IPAddressInfo;
                        }
                        else if (obj is string anotherIPAddressInfoString)
                        {
                                anotherIPAddressInfo = new IPAddressInfo(anotherIPAddressInfoString);
                        }
                        if (anotherIPAddressInfo == null)
                        {
                                return false;
                        }

                        var ipSectionStrings = _ipSectionStrings;
                        if (ipSectionStrings == null
                                || anotherIPAddressInfo.SectionsCount != this.SectionsCount)
                        {
                                return false;
                        }

                        var anotherIPSectionStrings = anotherIPAddressInfo.IPSectionStrings!;
                        for (var ipSectionStringIndex = this.SectionsCount - 1;
                                ipSectionStringIndex >= 0;
                                ipSectionStringIndex--)
                        {
                                var ipSectionString = ipSectionStrings[ipSectionStringIndex];
                                var anotherIPSectionString = anotherIPSectionStrings[ipSectionStringIndex];
                                if (!ipSectionString.Equals(IPAddressInfo.FuzzyMatchingKey)
                                        && !ipSectionString.Equals(anotherIPSectionString))
                                {
                                        return false;
                                }
                        }
                        return true;
                }

                public override int GetHashCode()
                {
                        if (_ipString?.Length > 0)
                        {
                                return _ipString.GetHashCode();
                        }
                        return 0;
                }

                #endregion
        }
}
