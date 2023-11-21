namespace Shared.RequestFeatures;

public class PagedList<T> : List<T>
{
	public PagginationMetaData MetaData { get; set; }

	public PagedList(List<T> items, int count, int pageNumber, int pageSize)
	{
		MetaData = new PagginationMetaData
		{
			TotalCount = count,
			PageSize = pageSize,
			CurrentPage = pageNumber,
			TotalPages = (int)Math.Ceiling(count / (double)pageSize)
		};

		AddRange(items);
	}
}
