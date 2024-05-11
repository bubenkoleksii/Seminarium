import { FC } from 'react';
import { JoiningRequestResponse } from '../../types/joiningRequestTypes';
import { Table } from "flowbite-react";
import { useMediaQuery } from 'react-responsive';

interface JoiningRequestsItemProps {
  item: JoiningRequestResponse;
}

const JoiningRequestsItem: FC<JoiningRequestsItemProps> = ({ item }) => {
  const isDesktopOrLaptop = useMediaQuery({ query: '(min-width: 1280px)' });

  return (
    <>
      {isDesktopOrLaptop
        ? <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
          <Table.Cell>{item.name}</Table.Cell>
          <Table.Cell>{item.requesterFullName}</Table.Cell>
          <Table.Cell>{item.requesterEmail}</Table.Cell>
          <Table.Cell>{item.requesterPhone}</Table.Cell>
          <Table.Cell>{item.region}</Table.Cell>
          <Table.Cell>{item.status}</Table.Cell>
        </Table.Row>
        : <>
          <h6>Назва {item.name}</h6>
        </>
      }
    </>
  );
};

export { JoiningRequestsItem };
