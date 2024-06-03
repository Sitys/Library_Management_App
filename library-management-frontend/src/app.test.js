import App  from "./App";
import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import axios from 'axios';
import axiosMockAdapter from 'axios-mock-adapter';

const mock = new axiosMockAdapter(axios);

describe('App Component', () => {
    afterEach(() => {
        mock.reset();
    });

    test('fetches and displays books on mount', async () => {
        const books = [
            { id: 1, title: 'Book 1', author: 'Author 1', copiesAvailable: 3 },
            { id: 2, title: 'Book 2', author: 'Author 2', copiesAvailable: 5 },
        ];
        
        mock.onGet('https://localhost:7128/api/LibraryManagment/GetBooks').reply(200, books);

        render(<App />);

        await waitFor(() => expect(screen.getByText('Book 1 by Author 1 - Copies Available: 3')).toBeInTheDocument());
        expect(screen.getByText('Book 2 by Author 2 - Copies Available: 5')).toBeInTheDocument();
    });

    test('adds a new book', async () => {
        const books = [
            { id: 1, title: 'Book 1', author: 'Author 1', copiesAvailable: 3 },
        ];
        
        mock.onGet('https://localhost:7128/api/LibraryManagment/GetBooks').reply(200, books);
        mock.onPost('https://localhost:7128/api/LibraryManagment/AddBook').reply(200);
        render(<App />);

        await waitFor(() => expect(screen.getByText('Book 1 by Author 1 - Copies Available: 3')).toBeInTheDocument());

        fireEvent.change(screen.getByPlaceholderText(/title/i), { target: { value: 'Book 2' } });
        fireEvent.change(screen.getByPlaceholderText(/author/i), { target: { value: 'Author 2' } });
        fireEvent.change(screen.getByPlaceholderText(/copies available/i), { target: { value: '5' } });

        fireEvent.click(screen.getByText(/add book/i));

        mock.onGet('https://localhost:7128/api/LibraryManagment/GetBooks').reply(200, [
            ...books,
            { id: 2, title: 'Book 2', author: 'Author 2', copiesAvailable: 5 },
        ]);

        await waitFor(() => expect(screen.getByText('Book 2 by Author 2 - Copies Available: 5')).toBeInTheDocument());
    });

    test('deletes a book', async () => {
        const books = [
            { id: 1, title: 'Book 1', author: 'Author 1', copiesAvailable: 3 },
        ];
        
        mock.onGet('https://localhost:7128/api/LibraryManagment/GetBooks').reply(200, books);
        mock.onDelete('https://localhost:7128/api/LibraryManagment/DeleteBook/1').reply(200);
        
        render(<App />);

        await waitFor(() => expect(screen.getByText('Book 1 by Author 1 - Copies Available: 3')).toBeInTheDocument());

        fireEvent.click(screen.getByText(/delete/i));

        mock.onGet('https://localhost:7128/api/LibraryManagment/GetBooks').reply(200, []);

        await waitFor(() => expect(screen.queryByText('Book 1 by Author 1 - Copies Available: 3')).not.toBeInTheDocument());
    });

    test('borrows a book', async () => {
        const books = [
            { id: 1, title: 'Book 1', author: 'Author 1', copiesAvailable: 3 },
        ];

        mock.onGet('https://localhost:7128/api/LibraryManagment/GetBooks').reply(200, books);
        mock.onPost('https://localhost:7128/api/LibraryManagment/LendBook/1/lend').reply(200);

        render(<App />);

        await waitFor(() => expect(screen.getByText('Book 1 by Author 1 - Copies Available: 3')).toBeInTheDocument());

        fireEvent.click(screen.getByText(/borrow/i));

        mock.onGet('https://localhost:7128/api/LibraryManagment/GetBooks').reply(200, [
            { id: 1, title: 'Book 1', author: 'Author 1', copiesAvailable: 2 },
        ]);

        await waitFor(() => expect(screen.getByText('Book 1 by Author 1 - Copies Available: 2')).toBeInTheDocument());
    });

    test('returns a book', async () => {
        const books = [
            { id: 1, title: 'Book 1', author: 'Author 1', copiesAvailable: 2 },
        ];

        mock.onGet('https://localhost:7128/api/LibraryManagment/GetBooks').reply(200, books);
        mock.onPost('https://localhost:7128/api/LibraryManagment/ReturnBook/1/return').reply(200);

        render(<App />);

        await waitFor(() => expect(screen.getByText('Book 1 by Author 1 - Copies Available: 2')).toBeInTheDocument());

        fireEvent.click(screen.getByText(/return/i));

        mock.onGet('https://localhost:7128/api/LibraryManagment/GetBooks').reply(200, [
            { id: 1, title: 'Book 1', author: 'Author 1', copiesAvailable: 3 },
        ]);

        await waitFor(() => expect(screen.getByText('Book 1 by Author 1 - Copies Available: 3')).toBeInTheDocument());
    });
});
