using System;

namespace Authentication.Web.Attributes
{
  [AttributeUsage(AttributeTargets.Method)]
  public class AllowAnonymousAttribute : Attribute
  {
  }
}