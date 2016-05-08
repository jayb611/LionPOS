﻿using LionPOSServiceContractModels.DomainContractsModel.warehouse;
using LionPOSServiceContractModels.ErrorContactModel;
using LionUtilities.SQLUtilitiesPkg.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LionPOSServiceContractModels.ControllerContractModel.warehouseManagement
{
    [DataContract]
    public class WarehouseCCM : ErrorCM
    {
        [DataMember]
        public List<warehouseDCM> warehouseList { get; set; }

        [DataMember]
        public PaginationCM Pagination { get; set; }

        [DataMember]
        public string FilterFieldModelJson { get; set; }

        [DataMember]
        public string SQLDataTypeJosn { get; set; }

        [DataMember]
        public List<FilterFieldsModel> FilterFieldsModel { get; set; }

        [DataMember]
        public string bulkActionName { get; set; }

        [DataMember]
        public string warehousesCodes { get; set; }

        [DataMember]
        public string InsertActionName { get; set; }

        public WarehouseCCM()
        {
            FilterFieldsModel = new List<LionUtilities.SQLUtilitiesPkg.Models.FilterFieldsModel>();
            warehouseList = new List<warehouseDCM>();
            Pagination = new PaginationCM();

            SQLDataTypeJosn = JsonConvert.SerializeObject(new SQLDataTypConversionModel());
        }
    }
}