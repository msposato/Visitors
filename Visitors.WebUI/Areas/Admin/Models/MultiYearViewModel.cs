using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Visitors.WebUI.Admin.Models
{
  public class MultiYearViewModel
  {
    private readonly HashSet<SelectListItem> _years = new HashSet<SelectListItem>();
    private readonly HashSet<SelectListItem> _months = new HashSet<SelectListItem>();

    public MultiYearViewModel()
    {
      _years.Add(new SelectListItem {Text = "2010 - 2011", Value = "2010"});
      _years.Add(new SelectListItem {Text = "2011 - 2012", Value = "2011"});
      _years.Add(new SelectListItem {Text = "2012 - 2013", Value = "2012"});
      _years.Add(new SelectListItem {Text = "2013 - 2014", Value = "2013"});
      _years.Add(new SelectListItem {Text = "2014 - 2015", Value = "2014"});
      _years.Add(new SelectListItem {Text = "2015 - 2016", Value = "2015"});
      _years.Add(new SelectListItem {Text = "2016 - 2017", Value = "2016"});
      _years.Add(new SelectListItem {Text = "2017 - 2018", Value = "2017"});
      _years.Add(new SelectListItem {Text = "2018 - 2019", Value = "2018"});

      _months.Add(new SelectListItem {Text = "September", Value = "9"});
      _months.Add(new SelectListItem { Text = "October", Value = "10" });
      _months.Add(new SelectListItem { Text = "November", Value = "11" });
      _months.Add(new SelectListItem { Text = "December", Value = "12" });
      _months.Add(new SelectListItem { Text = "January", Value = "1" });
      _months.Add(new SelectListItem { Text = "February", Value = "2" });
      _months.Add(new SelectListItem { Text = "March", Value = "3" });
      _months.Add(new SelectListItem { Text = "April", Value = "4" });
      _months.Add(new SelectListItem { Text = "May", Value = "5" });
      _months.Add(new SelectListItem { Text = "June", Value = "6" });
      _months.Add(new SelectListItem { Text = "July", Value = "7" });
      _months.Add(new SelectListItem { Text = "August", Value = "8" });
    }

    public int Year { get; set; }

    public int Month { get; set; }

    public DateTime BeginDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public virtual IEnumerable<SelectListItem> Years
    {
      get { return _years; }
    }

    public virtual IEnumerable<SelectListItem> Months
    {
      get { return _months; }
    }
  }
}