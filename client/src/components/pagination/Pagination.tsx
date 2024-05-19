import { FC } from 'react';

interface PaginationProps {
  totalCount: number;
  currentPage: number;
  onChangePage: (page: number) => void;
  limit: number;
}

const Pagination: FC<PaginationProps> = ({
  totalCount,
  currentPage,
  onChangePage,
  limit,
}) => {
  const pagesCount: number = Math.ceil(totalCount / limit);

  const firstPages: number[] = [1, 2];
  let lastPages = [pagesCount - 1, pagesCount];
  lastPages = lastPages.filter((item) => !firstPages.includes(item));

  const middlePage: number =
    currentPage < Math.ceil(pagesCount / 2)
      ? currentPage + Math.ceil(currentPage / 2)
      : currentPage + Math.floor((pagesCount - 1 - currentPage) / 2);

  const previous = () => {
    if (currentPage > 1) {
      onChangePage(currentPage - 1);
    }
  };

  const next = () => {
    if (currentPage < pagesCount) {
      onChangePage(currentPage + 1);
    }
  };

  const goPage = (page: number) => {
    onChangePage(page);
  };

  return (
    <div className="mb-8 mt-4 flex justify-center">
      {pagesCount > 0 && (
        <div className="flex">
          {currentPage > 1 && (
            <button
              className="mr-4 rounded-l-md bg-purple-950 px-4 py-2 text-white hover:bg-purple-800 focus:outline-none"
              onClick={previous}
            >
              {'<'}
            </button>
          )}

          <div className="flex flex-wrap items-end gap-2">
            {firstPages.map((page) => (
              <button
                key={page}
                className={
                  page === currentPage
                    ? 'h-12 rounded bg-purple-950 px-4 py-2 text-white'
                    : 'h-12 cursor-pointer rounded bg-white px-4 py-2 text-gray-800 transition-colors hover:bg-gray-200'
                }
                onClick={() => goPage(page)}
              >
                {page}
              </button>
            ))}

            {currentPage > firstPages[firstPages.length - 1] &&
              currentPage < lastPages[0] && (
                <>
                  {currentPage - firstPages[firstPages.length - 1] > 1 && (
                    <span>...</span>
                  )}

                  <button
                    className="h-12 rounded bg-purple-950 px-4 py-2 text-white"
                    onClick={() => goPage(currentPage)}
                  >
                    {currentPage}
                  </button>

                  {currentPage + 1 < lastPages[0] && (
                    <button
                      className="h-12 cursor-pointer rounded bg-white px-4 py-2 text-gray-800 transition-colors hover:bg-gray-200"
                      onClick={() => goPage(currentPage + 1)}
                    >
                      {currentPage + 1}
                    </button>
                  )}

                  {middlePage - currentPage > 2 && <span>...</span>}

                  {middlePage < lastPages[0] &&
                    middlePage > currentPage + 1 && (
                      <button
                        className="h-12 cursor-pointer rounded bg-white px-4 py-2 text-gray-800 transition-colors hover:bg-gray-200"
                        onClick={() => goPage(middlePage)}
                      >
                        {middlePage}
                      </button>
                    )}
                </>
              )}

            {lastPages &&
              lastPages.length > 0 &&
              lastPages[0] - middlePage !== 1 && <span>...</span>}
            {lastPages &&
              lastPages.length > 0 &&
              lastPages.map((page) => (
                <button
                  key={page}
                  className={
                    page === currentPage
                      ? 'h-12 rounded bg-purple-950 px-4 py-2 text-white'
                      : 'h-12 cursor-pointer rounded bg-white px-4 py-2 text-gray-800 transition-colors hover:bg-gray-200'
                  }
                  onClick={() => goPage(page)}
                >
                  {page}
                </button>
              ))}
          </div>

          {currentPage < pagesCount && (
            <button
              className="ml-4 rounded-r-md bg-purple-950 px-4 py-2 text-white hover:bg-purple-800 focus:outline-none"
              onClick={next}
            >
              {'>'}
            </button>
          )}
        </div>
      )}
    </div>
  );
};

export { Pagination };
