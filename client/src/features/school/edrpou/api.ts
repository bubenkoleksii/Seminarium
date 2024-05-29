import type { Company } from './types';
import axios from 'axios';
import xml2js from 'xml2js';

type ApiResponse = {
  export: {
    company: Company;
  };
};

export const fetchData = async (id: string): Promise<Company | null> => {
  try {
    const url = getUrl(id);
    console.log('url', url);

    const response = await axios.get(url, {
      responseType: 'text', // Отримання даних у вигляді тексту
      headers: {
        'Content-Type': 'text/xml;charset=windows-1251',
        'Access-Control-Allow-Origin': '*',
      },
      transformResponse: [(data) => data], // Запобігання автоматичному перетворенню відповіді
    });

    console.log('text', response);

    const decodedData = new TextDecoder('windows-1251').decode(new Uint8Array(response.data.split('').map(char => char.charCodeAt(0))));

    const parser = new xml2js.Parser();
    const parsedResult = await new Promise<ApiResponse>((resolve, reject) => {
      parser.parseString(decodedData, (err, result) => {
        if (err) {
          reject(err);
        } else {
          resolve(result);
        }
      });
    });

    if (parsedResult.export && parsedResult.export.company) {
      return parsedResult.export.company;
    } else {
      throw new Error('Company not found or invalid response');
    }
  } catch (error) {
    throw new Error('Failed to fetch data');
  }
};

