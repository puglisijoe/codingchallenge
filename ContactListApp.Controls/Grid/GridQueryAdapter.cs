using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ContactListApp.Model;
using Microsoft.EntityFrameworkCore;

namespace ContactListApp.Controls {
    public class GridQueryAdapter {
        private readonly IContactFilters _controls;


        private readonly Dictionary<ContactFilterColumns, Expression<Func<Contact, string>>> _expressions
            = new Dictionary<ContactFilterColumns, Expression<Func<Contact, string>>> {
                { ContactFilterColumns.City, c => c.City },
                { ContactFilterColumns.Phone, c => c.Phone },
                { ContactFilterColumns.Name, c => c.LastName },
                { ContactFilterColumns.State, c => c.State },
                { ContactFilterColumns.Street, c => c.Street },
                { ContactFilterColumns.ZipCode, c => c.ZipCode }
            };


        private readonly Dictionary<ContactFilterColumns, Func<IQueryable<Contact>, IQueryable<Contact>>>
            _filterQueries;

        public GridQueryAdapter(IContactFilters controls) {
            _controls = controls;
            _filterQueries = new Dictionary<ContactFilterColumns, Func<IQueryable<Contact>, IQueryable<Contact>>> {
                { ContactFilterColumns.City, cs => cs.Where(c => c.City.Contains(_controls.FilterText)) },
                { ContactFilterColumns.Phone, cs => cs.Where(c => c.Phone.Contains(_controls.FilterText)) }, {
                    ContactFilterColumns.Name,
                    cs => cs.Where(c =>
                        c.FirstName.Contains(_controls.FilterText) || c.LastName.Contains(_controls.FilterText))
                },
                { ContactFilterColumns.State, cs => cs.Where(c => c.State.Contains(_controls.FilterText)) },
                { ContactFilterColumns.Street, cs => cs.Where(c => c.Street.Contains(_controls.FilterText)) },
                { ContactFilterColumns.ZipCode, cs => cs.Where(c => c.ZipCode.Contains(_controls.FilterText)) }
            };
        }

        public async Task<ICollection<Contact>> FetchAsync(IQueryable<Contact> query) {
            query = FilterAndQuery(query);
            await CountAsync(query);
            List<Contact> collection = await FetchPageQuery(query)
                .ToListAsync();
            _controls.PageHelper.PageItems = collection.Count;
            return collection;
        }

        public async Task CountAsync(IQueryable<Contact> query) {
            _controls.PageHelper.TotalItemCount = await query.CountAsync();
        }

        public IQueryable<Contact> FetchPageQuery(IQueryable<Contact> query) {
            return query
                .Skip(_controls.PageHelper.Skip)
                .Take(_controls.PageHelper.PageSize)
                .AsNoTracking();
        }

        private IQueryable<Contact> FilterAndQuery(IQueryable<Contact> root) {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(_controls.FilterText)) {
                Func<IQueryable<Contact>, IQueryable<Contact>> filter = _filterQueries[_controls.FilterColumn];
                sb.Append($"Filter: '{_controls.FilterColumn}' ");
                root = filter(root);
            }

            Expression<Func<Contact, string>> expression = _expressions[_controls.SortColumn];
            sb.Append($"Sort: '{_controls.SortColumn}' ");
            if (_controls.SortColumn == ContactFilterColumns.Name && _controls.ShowFirstNameFirst) {
                sb.Append("(first name first) ");
                expression = c => c.FirstName;
            }

            string sortDir = _controls.SortAscending ? "ASC" : "DESC";
            sb.Append(sortDir);
            return _controls.SortAscending ? root.OrderBy(expression) : root.OrderByDescending(expression);
        }
    }
}