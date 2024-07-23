using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common.Elasticsearch;

namespace FWBS.OMS.UI.Elasticsearch
{
    public class FacetsBuilder
    {
        private Panel _panel;
        private System.Windows.Forms.ToolTip _toolTip;

        public FacetsBuilder(Panel panel)
        {
            _panel = panel;
            _toolTip = new ToolTip();
        }

        public void Build(List<AggregationBucket> facetGroups, EventHandler handler, List<FacetLabel> facetLabels)
        {
            ClearPanel(handler);

            foreach (AggregationBucket bucket in facetGroups.OrderByDescending(b => b.Order).ThenByDescending(b => b.Title))
            {
                var facetGroup = CreateFacetGroup(bucket, handler, facetLabels);
                _panel.Controls.Add(facetGroup);
            }
        }

        private void ClearPanel(EventHandler handler)
        {
            foreach (ucFacetGroup facetGroup in _panel.Controls)
            {
                foreach (var control in facetGroup.Controls)
                {
                    if (control is ucFacetItem)
                    {
                        var facetItem = control as ucFacetItem;
                        facetItem.CheckedChanged -= handler;
                    }
                }
            }
            _panel.Controls.Clear();
        }

        private ucFacetGroup CreateFacetGroup(AggregationBucket group, EventHandler handler, List<FacetLabel> facetLabels)
        {
            var facetGroup = new ucFacetGroup
            {
                Dock = DockStyle.Top
            };

            for (int i = group.Aggregations.Count-1; i >= 0 ; i--)
            {
                var facetItem = new ucFacetItem
                {
                    BackColor = System.Drawing.Color.FromArgb(244, 244, 244),
                    Field = group.Field,
                    Dock = DockStyle.Top,
                    Number = group.Aggregations[i].Number,
                    Value = group.Aggregations[i].Value
                };

                _toolTip.SetToolTip(facetItem, facetItem.Text);

                var appliedFilter = facetLabels.FirstOrDefault(label =>
                    label.Field == group.Field && label.Value == group.Aggregations[i].Value);
                if (appliedFilter != null)
                {
                    facetItem.Checked = true;
                    facetItem.Key = appliedFilter.Key;
                }

                facetItem.CheckedChanged += handler;
                facetGroup.Controls.Add(facetItem);
            }

            var groupTitle = new Label
            {
                AutoSize = true,
                Dock = DockStyle.Top,
                Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F),
                Margin = new Padding(0),
                Padding = new Padding(0,0,0,3),
                Text = group.Title
            };

            _toolTip.SetToolTip(groupTitle, groupTitle.Text);
            facetGroup.Controls.Add(groupTitle);

            return facetGroup;
        }
    }
}
