using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models

{
    /// <summary>
    /// A class to store contact details and nominated place details from customer
    /// </summary>
    public class CustomerModel
    {
        // Customer first name
        public string CusFirstName { get; set; }

        // Customer last name
        public string CusLastName { get; set;}

        // Customer contact email
        public string CusEmail { get; set;}
        
        // Nominated title of the new place 
        public string NominatedTitle { get; set; }
        
        // Nominated image url
        public string NominatedImage { get; set; }
        
        // Nominated noise level
        public int NominatedNoiseLevel { get; set;}
        
        // Nominated neighborhood
        public string NominatedNeighborhoodP { get; set; }
        
        // Nominated map details 
        public string NominatedMapDetails { get; set;}
        
        // Overrides the ToString method to serialize the object to JSON.
        public override string ToString() => JsonSerializer.Serialize<CustomerModel>(this);
    }
    
    
    
    
}
