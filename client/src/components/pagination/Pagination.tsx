import { FC } from 'react';

interface PaginationProps {
  totalCount: number;
  currentPage: number;
  onChangePage: (page: number) => void;
  limit: number;
}

const Pagination: FC<PaginationProps> = (
  {
    totalCount,
    currentPage,
    onChangePage,
    limit
  }) => {
  const pagesCount: number = Math.ceil(totalCount / limit);

  const firstPages: number[] = [1, 2];
  const lastPages: number[] = [pagesCount - 1, pagesCount];
  const middlePage: number = currentPage < Math.ceil(pagesCount / 2)
    ? currentPage + Math.ceil(currentPage / 2)
    : currentPage + Math.floor((pagesCount - 1 - currentPage) / 2);

  const previous = () => {
    if (currentPage > 1) {
      onChangePage(currentPage - 1);
    }
  }

  const next = () => {
    if (currentPage < pagesCount) {
      onChangePage(currentPage + 1);
    }
  }

  const goPage = (page: number) => {
    onChangePage(page);
  }

  return (
    <div className="flex justify-center mt-4 mb-8">
      {pagesCount > 0 &&
        <div className="flex">
          {currentPage > 1 &&
            <button
              className="rounded-l-md px-4 py-2 focus:outline-none mr-4 bg-purple-950 text-white hover:bg-purple-800"
              onClick={previous}
            >
              {"<"}
            </button>
          }

          <div className="flex items-end gap-2 flex-wrap">
            {firstPages.map((page) => (
              <button
                key={page}
                className={page === currentPage ? "h-12 bg-purple-950 text-white px-4 py-2 rounded" : "text-gray-800 h-12 bg-white px-4 py-2 rounded cursor-pointer transition-colors hover:bg-gray-200"}
                onClick={() => goPage(page)}
              >
                {page}
              </button>
            ))}

            {currentPage > firstPages[firstPages.length - 1] && currentPage < lastPages[0]
              && <>
                {currentPage - firstPages[firstPages.length - 1] > 1 &&
                  <span>...</span>
                }

                <button
                  className="h-12 bg-purple-950 text-white px-4 py-2 rounded"
                  onClick={() => goPage(currentPage)}
                >
                  {currentPage}
                </button>

                {currentPage + 1 < lastPages[0] &&
                  <button
                    className="text-gray-800 h-12 bg-white px-4 py-2 rounded cursor-pointer transition-colors hover:bg-gray-200"
                    onClick={() => goPage(currentPage + 1)}
                  >
                    {currentPage + 1}
                  </button>
                }

                {middlePage - currentPage > 2 &&
                  <span>...</span>
                }

                {middlePage < lastPages[0] && middlePage > currentPage + 1 &&
                  <button
                    className="text-gray-800 h-12 bg-white px-4 py-2 rounded cursor-pointer transition-colors hover:bg-gray-200"
                    onClick={() => goPage(middlePage)}
                  >
                    {middlePage}
                  </button>
                }
              </>
            }

            {lastPages[0] - middlePage !== 1 &&
              <span>...</span>
            }
            {lastPages.map((page) => (
              <button
                key={page}
                className={page === currentPage ? "h-12 bg-purple-950 text-white px-4 py-2 rounded" : "text-gray-800 h-12 bg-white px-4 py-2 rounded cursor-pointer transition-colors hover:bg-gray-200"}
                onClick={() => goPage(page)}
              >
                {page}
              </button>
            ))}
          </div>

          {currentPage < pagesCount &&
            <button
              className="rounded-r-md px-4 py-2 focus:outline-none ml-4 bg-purple-950 text-white hover:bg-purple-800"
              onClick={next}
            >
              {">"}
            </button>
          }
        </div>
      }
    </div>
  );
};

export { Pagination };
