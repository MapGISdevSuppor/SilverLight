// Type: ZDIMS.BaseLib.CNetAnalyse
// Assembly: ZDIMS1.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: E:\4-Demo\SilverlightDemo\SilverlightDemo\Bin\Debug\ZDIMS1.0.dll

namespace ZDIMS.BaseLib
{
  public class CNetAnalyse
  {
    private CGdbInfo gdbInfo;
    private string netLayerName;
    private string flgType;
    private string requestDots;
    private string barrierDots;
    private string netWeight;
    private double nearDis;
    private bool requireGeomDots;
    private string analysTypeParam;

    public string AnalysTypeParam
    {
      get
      {
        return this.analysTypeParam;
      }
      set
      {
        this.analysTypeParam = value;
      }
    }

    public CGdbInfo GdbInfo
    {
      get
      {
        return this.gdbInfo;
      }
      set
      {
        this.gdbInfo = value;
      }
    }

    public string NetLayerName
    {
      get
      {
        return this.netLayerName;
      }
      set
      {
        this.netLayerName = value;
      }
    }

    public string FlgType
    {
      get
      {
        return this.flgType;
      }
      set
      {
        this.flgType = value;
      }
    }

    public string RequestDots
    {
      get
      {
        return this.requestDots;
      }
      set
      {
        this.requestDots = value;
      }
    }

    public string BarrierDots
    {
      get
      {
        return this.barrierDots;
      }
      set
      {
        this.barrierDots = value;
      }
    }

    public string NetWeight
    {
      get
      {
        return this.netWeight;
      }
      set
      {
        this.netWeight = value;
      }
    }

    public double NearDis
    {
      get
      {
        return this.nearDis;
      }
      set
      {
        this.nearDis = value;
      }
    }

    public bool RequireGeomDots
    {
      get
      {
        return this.requireGeomDots;
      }
      set
      {
        this.requireGeomDots = value;
      }
    }
  }
}
