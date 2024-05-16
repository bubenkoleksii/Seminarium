import { FC } from 'react';
import { useTranslations } from 'next-intl';

interface LimitProps {
  limitOptions: number[];
  currentLimit: number;
  onChangeLimit: (limit: number) => void;
}

const Limit: FC<LimitProps> = ({ limitOptions, currentLimit, onChangeLimit }) => {
  const t = useTranslations('Pagination')

  return (
    <div className="flex flex-col w-[300px] items-center">
      <label className="block mb-1 font-medium text-gray-700 text-center relative">
        {t('limit')}
      </label>

      <div className="flex">
        {limitOptions.map((option) => (
          <button
            key={option}
            className={option === currentLimit ?
              "mx-2 px-4 py-2 bg-purple-950 text-white rounded cursor-pointer"
              : "mx-2 px-4 py-2 bg-white text-gray-800 rounded cursor-pointer hover:bg-gray-200"
            }
            onClick={() => onChangeLimit(option)}
          >
            {option}
          </button>
        ))}
      </div>
    </div>
  );
};

export { Limit };
