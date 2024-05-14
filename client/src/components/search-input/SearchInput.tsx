import React, { ChangeEvent, FC, FormEvent, useState } from 'react';
import { useTranslations } from 'next-intl';

interface SearchInputProps {
  placeholder?: string;
  onSubmit: (searchValue: string) => void;
  size?: 'small' | 'medium' | 'large';
  maxLength?: number;
}

const SearchInput: FC<SearchInputProps> =  ({ placeholder, onSubmit, maxLength, size = 'medium' }) => {
  const t = useTranslations('SearchInput');
  const [searchValue, setSearchValue] = useState<string>('');

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    setSearchValue(e.target.value);
  };

  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    onSubmit(searchValue);
  };

  const getSizeClassName = () => {
    switch (size) {
      case 'small':
        return 'px-3 py-1 text-sm';
      case 'large':
        return 'px-6 py-3 text-lg';
      default:
        return 'px-4 py-2 text-base';
    }
  };

  return (
    <form onSubmit={handleSubmit} className="max-w-xl mx-auto">
      <div className="relative">
        <div className="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
          <svg
            className="w-4 h-4 text-gray-500 dark:text-gray-400"
            aria-hidden="true"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 20 20"
          >
            <path
              stroke="currentColor"
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth="2"
              d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z"
            />
          </svg>
        </div>
        <input
          type="search"
          id="default-search"
          maxLength={maxLength}
          className={`block w-full h-[55px] p-4 ps-10 text-gray-900 border border-gray-300 
            rounded-lg bg-gray-50 focus:ring-purple-950 focus:border-purple-950 dark:bg-gray-700 
            ${getSizeClassName()}`
          }
          placeholder={placeholder || ''}
          value={searchValue}
          onChange={handleInputChange}
          required
        />
        <button
          type="submit"
          className={`text-white absolute end-2.5 bottom-2 bg-purple-950 hover:bg-purple-900 
            focus:outline-none focus:ring-blue-300 font-medium rounded-lg p-2`
          }
        >
          {t('label')}
        </button>
      </div>
    </form>
  );
};

export { SearchInput };
