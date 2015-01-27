// Type: System.Net.UploadStringCompletedEventArgs
// Assembly: System.Net, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// Assembly location: C:\Program Files\Reference Assemblies\Microsoft\Framework\Silverlight\v4.0\System.Net.dll

using System;
using System.ComponentModel;

namespace System.Net
{
  public class UploadStringCompletedEventArgs : AsyncCompletedEventArgs
  {
    private string m_Result;

    public string Result
    {
      get
      {
        this.RaiseExceptionIfNecessary();
        return this.m_Result;
      }
    }

    internal UploadStringCompletedEventArgs(string result, Exception exception, bool cancelled, object userToken)
      : base(exception, cancelled, userToken)
    {
      this.m_Result = result;
    }
  }
}
