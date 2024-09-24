function GroupDataTableByColumnsIndexArray(
    tableSelector,
    groupColumnIndex,
    ColumnsToSumArray
) {
    debugger;
    const ColumnsArray = Array.isArray(ColumnsToSumArray)
        ? [...ColumnsToSumArray].sort((a, b) => a - b)
        : typeof ColumnsToSumArray === "number"
            ? [ColumnsToSumArray]
            : [];
    
    if ($.fn.DataTable.isDataTable(tableSelector)) {
        $(tableSelector).DataTable().destroy();
    }

    if (groupColumnIndex != "") {
        var table = $(tableSelector).DataTable({
            responsive: true,
            order: [[groupColumnIndex, "desc"]],
            displayLength: 25,
            columnDefs: [{ targets: parseInt(groupColumnIndex), visible: false }],
            drawCallback: function (settings) {
                var last = null;
                var api = this.api();
                var rows = api.rows().nodes();
                var groupSums = Array(ColumnsArray.length).fill(0);
                var totalColumnsCount = api.columns(":visible").count();

                api
                    .column(groupColumnIndex)
                    .data()
                    .each(function (group, i) {
                        if (last !== group) {
                            if (last !== null && ColumnsArray.length > 0) {
                                var sumRow = GetSumRow(
                                    tableSelector,
                                    ColumnsArray,
                                    groupSums,
                                    totalColumnsCount
                                );
                                $(rows).eq(i).before(sumRow);
                            }

                            // Add the group header row
                            var groupRow = GetGroupRow(group, totalColumnsCount);
                            $(rows).eq(i).before(groupRow);

                            last = group;
                            groupSums = Array(ColumnsArray.length).fill(0); // Reset the sums for the new group
                        }

                        // Add the current row's values to the group sums
                        if (ColumnsArray.length > 0) {
                            var rowData = api.row($(rows).eq(i)).data();
                            for (var j = 0; j < ColumnsArray.length; j++) {
                                groupSums[j] += parseFloat(rowData[ColumnsArray[j]]) || 0;
                            }

                            // If this is the last row, add the sum row for the last group
                            if (i === rows.length - 1) {
                                var sumRow = GetSumRow(
                                    tableSelector,
                                    ColumnsArray,
                                    groupSums,
                                    totalColumnsCount
                                );
                                $(rows).eq(i).after(sumRow);
                            }
                        }
                    });
            },
        });
        table.draw(true)
    } else {
        var table = $(tableSelector).DataTable({
            responsive: true,
            displayLength: 10,
            order: [[0, "desc"]],
        });
        table.draw(true)
    }
}

function GetSumRow(tableSelector, ColumnsArray, groupSums, totalColumnsCount) {
    var sumRow =
        '<tr class="sum-row" style="background-color:#f0f8ff;font-weight:bold;color:#337ab7;height:20px">';

    sumRow += '<td colspan="' + ColumnsArray[0] + '"></td>';

    for (var j = 0; j < groupSums.length; j++) {
        // check if corresponding TH Element is hidden
        var thElement = $(tableSelector + " th").eq(ColumnsArray[j]);
        var isDisplayed = thElement.css("display") !== "none";
        if (isDisplayed) {
            sumRow += "<td>" + groupSums[j].toFixed(2) + "</td>";
        }
    }

    var remianCells =
        totalColumnsCount - ColumnsArray[ColumnsArray.length - 1] - 1;

    if (remianCells > 0) {
        sumRow += '<td colspan="' + remianCells + '"></td>';
    }

    sumRow += "</tr>";
    return sumRow;
}

function GetGroupRow(groupName, totalColumnsCount) {
    return (
        '<tr class="group"><td colspan="' +
        totalColumnsCount +
        '" style="padding:10px;background-color:#EEE;">' +
        groupName +
        "</td></tr>"
    );
}

function FillGroupSelectListWithColumns(TableSelector ,SelectListSelector , lang) {
    var table = $(TableSelector).DataTable();
    var columns = table.columns().header().toArray();
    var select = $(SelectListSelector);

    select.empty();

    if (lang == "ar") {
        select.append($('<option></option>').attr('value', '').text('تجميع حسب العمود'));
    }
    else {
        select.append($('<option></option>').attr('value', '').text('Group By Column'));
    }
    columns.forEach(function (column, index) {
        // Get the first cell in the current column  to skip columns that contains number
        //Because we dont want to group with number cols
        var firstCellText = table.column(index).data()[0]; 

        if (index > 0 && isNaN(+firstCellText)) {
            var columnText = $(column).text().trim();
            select.append($('<option></option>').attr('value', index).text(columnText));
        }
    });

    if (lang == "ar") {
            select.append($('<option></option>').attr('value', '').text('بدون تجميع'));
    }
    else {
        select.append($('<option></option>').attr('value', '').text('No Group'));
    }
   

}


/*

    In Views 
    ----------------------
     <div class="x_panel" style="overflow-x:auto">
     <select class="form-control" style="width:30%" id="groupSelect">
         <option value="" selected>@MyTypeLocalizer.GetLocalizedHtmlString("Group By Column")</option>
     </select>
 </div>
 
   <script src="~/js/datatableextend-buttons.js"></script>
  <script src="~/js/groupdatatablebycolumnsindexarray.js"></script>
  <script>

      $(function () {
          FillGroupSelectListWithColumns("#example", "#groupSelect", '@lang')
      })

      $("#groupSelect").on('change', (e) => {
          GroupDataTableByColumnsIndexArray("#example", e.target.value, [9, 10])
          DataTableExtendButton('@lang')
      })
</script>

*/


