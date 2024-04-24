package main

import (
	"bufio"
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"log"
	"mime/multipart"
	"net"
	"net/http"
	"net/url"
	"os"
	"strings"
)

type RequestModel struct {
	Room string `json:"room"`
	Data struct {
		Monaco struct {
			Type    string `json:"type"`
			Content string `json:"content"`
		} `json:"monaco"`
	} `json:"data"`
}

func main() {
	log.Println("Start server")
	ln, err := net.Listen("tcp", ":4321")
	if err != nil {
		log.Fatalf("Failed to listen: %v", err)
	}
	defer ln.Close()

	for {
		conn, err := ln.Accept()
		if err != nil {
			log.Printf("Failed to accept connection: %v", err)
			continue
		}
		go handleConnection(conn)
	}
}

func handleConnection(conn net.Conn) {
	defer conn.Close()

	reader := bufio.NewReader(conn)

	for {
		line, err := reader.ReadString('\n')
		if err != nil {
			if err != io.EOF {
				log.Printf("Error reading request: %v", err)
			}
			return
		}
		if line == "\r\n" {
			break
		}
	}

	var body []byte
	for {
		buf := make([]byte, 1024)
		n, err := reader.Read(buf)
		if err != nil {
			if err != io.EOF {
				log.Printf("Error reading request body: %v", err)
			}
			break
		}
		body = append(body, buf[:n]...)
	}

	var req RequestModel
	_ = json.Unmarshal(body, &req)

	projectId, prefix, fileName := ParseRoom(req.Room)
	_ = UploadFile(projectId, prefix, fileName, []byte(req.Data.Monaco.Content))
}

func UploadFile(projectID string, prefix string, fileName string, fileContent []byte) error {
	client := &http.Client{}

	baseURL := os.Getenv("FILE_SERVICE_URL")
	//baseURL := "http://localhost:5003"

	if baseURL == "" {
		return fmt.Errorf("BASE_URL environment variable is not set")
	}

	u, err := url.Parse(baseURL)
	if err != nil {
		return fmt.Errorf("error parsing base URL: %v", err)
	}
	u.Path = fmt.Sprintf("/projects/%s/files", projectID)

	body := &bytes.Buffer{}
	writer := multipart.NewWriter(body)

	part, err := writer.CreateFormFile("file", fileName)
	if err != nil {
		return fmt.Errorf("error creating form file: %v", err)
	}
	if _, err := part.Write(fileContent); err != nil {
		return fmt.Errorf("error writing file content to part: %v", err)
	}

	if prefix != "" {
		if err := writer.WriteField("prefix", prefix); err != nil {
			return fmt.Errorf("error writing prefix field: %v", err)
		}
	}

	if err := writer.Close(); err != nil {
		return fmt.Errorf("error closing multipart writer: %v", err)
	}

	req, err := http.NewRequest("POST", u.String(), body)
	if err != nil {
		return fmt.Errorf("error creating request: %v", err)
	}
	req.Header.Set("Content-Type", writer.FormDataContentType())

	resp, err := client.Do(req)
	if err != nil {
		return fmt.Errorf("error making request: %v", err)
	}
	defer resp.Body.Close()

	if resp.StatusCode != http.StatusOK {
		return fmt.Errorf("unexpected status code: %d", resp.StatusCode)
	}

	return nil
}

func ParseRoom(room string) (string, string, string) {
	parts := strings.Split(room, ":")
	if len(parts) != 3 {
		return "", "", ""
	}
	return parts[0], parts[1], parts[2]
}
