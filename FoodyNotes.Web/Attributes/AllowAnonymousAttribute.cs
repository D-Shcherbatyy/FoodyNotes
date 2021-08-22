using System;

namespace FoodyNotes.Web.Attributes
{
  [AttributeUsage(AttributeTargets.Method)]
  public class AllowAnonymousAttribute : Attribute
  {
  }
}