using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCProject10._5._2019.Models
{
    [MetadataType(typeof(product.productAnnotation))]
    public partial class product
    {
        sealed class productAnnotation
        {
            public int prod_id { get; set; }
            [Required]
            [Display(Name ="Product Name")]
            public string prod_name { get; set; }
            [Required]
            [Display(Name = "Product Image")]
            public string prod_image { get; set; }
            [Required]
            [Display(Name = "Description")]
            public string prod_description { get; set; }
            [Required]
            public Nullable<int> price { get; set; }
            [Required]
            public Nullable<int> stock { get; set; }
            [Display(Name = "Category Of Product")]
            public Nullable<int> Cat_ID { get; set; }
        }
    }
}