--
-- PostgreSQL database dump
--

-- Dumped from database version 17.6
-- Dumped by pg_dump version 17.5

-- Started on 2025-10-21 21:27:46

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 865 (class 1247 OID 16421)
-- Name: repair_status_enum; Type: TYPE; Schema: public; Owner: postgres
--

CREATE TYPE public.repair_status_enum AS ENUM (
    'принят',
    'выполнен',
    'выдан'
);


ALTER TYPE public.repair_status_enum OWNER TO postgres;

--
-- TOC entry 227 (class 1255 OID 16465)
-- Name: update_repair_status_on_completion(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.update_repair_status_on_completion() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Если completion_date установлена (не NULL) и статус еще не "выдан"
    IF NEW.completion_date IS NOT NULL AND NEW.status <> 'выдан' THEN
        NEW.status := 'выдан';
    END IF;
    
    -- Если completion_date сбрасывается в NULL и статус "выдан"
    IF NEW.completion_date IS NULL AND OLD.completion_date IS NOT NULL AND NEW.status = 'выдан' THEN
        NEW.status := 'принят'; -- или 'выполнен', в зависимости от бизнес-логики
    END IF;
    
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.update_repair_status_on_completion() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 16390)
-- Name: clients; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.clients (
    client_id integer NOT NULL,
    client_type integer NOT NULL,
    full_name character varying(100) NOT NULL,
    contact_phone character varying(20) NOT NULL,
    email character varying(100),
    organization_name character varying(100),
    client_notes text,
    CONSTRAINT clients_client_type_check CHECK ((client_type = ANY (ARRAY[0, 1])))
);


ALTER TABLE public.clients OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16389)
-- Name: clients_client_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.clients_client_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.clients_client_id_seq OWNER TO postgres;

--
-- TOC entry 4843 (class 0 OID 0)
-- Dependencies: 217
-- Name: clients_client_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.clients_client_id_seq OWNED BY public.clients.client_id;


--
-- TOC entry 220 (class 1259 OID 16400)
-- Name: devices; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.devices (
    device_id integer NOT NULL,
    device_type character varying(50) NOT NULL,
    manufacturer character varying(50) NOT NULL,
    model_number character varying(50) NOT NULL,
    serial_number character varying(50) NOT NULL,
    completeness text NOT NULL,
    fault_description text NOT NULL,
    device_notes text
);


ALTER TABLE public.devices OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16399)
-- Name: devices_device_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.devices_device_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.devices_device_id_seq OWNER TO postgres;

--
-- TOC entry 4844 (class 0 OID 0)
-- Dependencies: 219
-- Name: devices_device_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.devices_device_id_seq OWNED BY public.devices.device_id;


--
-- TOC entry 222 (class 1259 OID 16409)
-- Name: receipts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.receipts (
    receipt_id integer NOT NULL,
    client_id integer NOT NULL,
    doc_path character varying(255) NOT NULL
);


ALTER TABLE public.receipts OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 16408)
-- Name: receipts_receipt_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.receipts_receipt_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.receipts_receipt_id_seq OWNER TO postgres;

--
-- TOC entry 4845 (class 0 OID 0)
-- Dependencies: 221
-- Name: receipts_receipt_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.receipts_receipt_id_seq OWNED BY public.receipts.receipt_id;


--
-- TOC entry 224 (class 1259 OID 16428)
-- Name: repairs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.repairs (
    repair_id integer NOT NULL,
    device_id integer NOT NULL,
    receipt_id integer NOT NULL,
    work_performed text NOT NULL,
    acceptance_date date NOT NULL,
    completion_date date,
    status public.repair_status_enum DEFAULT 'принят'::public.repair_status_enum NOT NULL,
    repair_notes text
);


ALTER TABLE public.repairs OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 16427)
-- Name: repairs_repair_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.repairs_repair_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.repairs_repair_id_seq OWNER TO postgres;

--
-- TOC entry 4846 (class 0 OID 0)
-- Dependencies: 223
-- Name: repairs_repair_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.repairs_repair_id_seq OWNED BY public.repairs.repair_id;


--
-- TOC entry 226 (class 1259 OID 16448)
-- Name: sparepart; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sparepart (
    sparepart_id integer NOT NULL,
    repair_id integer,
    name character varying(100) NOT NULL,
    price numeric(10,2) NOT NULL,
    quantity integer DEFAULT 1
);


ALTER TABLE public.sparepart OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 16447)
-- Name: sparepart_sparepart_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.sparepart_sparepart_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.sparepart_sparepart_id_seq OWNER TO postgres;

--
-- TOC entry 4847 (class 0 OID 0)
-- Dependencies: 225
-- Name: sparepart_sparepart_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.sparepart_sparepart_id_seq OWNED BY public.sparepart.sparepart_id;


--
-- TOC entry 4665 (class 2604 OID 16393)
-- Name: clients client_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients ALTER COLUMN client_id SET DEFAULT nextval('public.clients_client_id_seq'::regclass);


--
-- TOC entry 4666 (class 2604 OID 16403)
-- Name: devices device_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.devices ALTER COLUMN device_id SET DEFAULT nextval('public.devices_device_id_seq'::regclass);


--
-- TOC entry 4667 (class 2604 OID 16412)
-- Name: receipts receipt_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.receipts ALTER COLUMN receipt_id SET DEFAULT nextval('public.receipts_receipt_id_seq'::regclass);


--
-- TOC entry 4668 (class 2604 OID 16431)
-- Name: repairs repair_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.repairs ALTER COLUMN repair_id SET DEFAULT nextval('public.repairs_repair_id_seq'::regclass);


--
-- TOC entry 4670 (class 2604 OID 16451)
-- Name: sparepart sparepart_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sparepart ALTER COLUMN sparepart_id SET DEFAULT nextval('public.sparepart_sparepart_id_seq'::regclass);


--
-- TOC entry 4674 (class 2606 OID 16398)
-- Name: clients clients_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clients
    ADD CONSTRAINT clients_pkey PRIMARY KEY (client_id);


--
-- TOC entry 4677 (class 2606 OID 16407)
-- Name: devices devices_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.devices
    ADD CONSTRAINT devices_pkey PRIMARY KEY (device_id);


--
-- TOC entry 4681 (class 2606 OID 16414)
-- Name: receipts receipts_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.receipts
    ADD CONSTRAINT receipts_pkey PRIMARY KEY (receipt_id);


--
-- TOC entry 4684 (class 2606 OID 16436)
-- Name: repairs repairs_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.repairs
    ADD CONSTRAINT repairs_pkey PRIMARY KEY (repair_id);


--
-- TOC entry 4687 (class 2606 OID 16454)
-- Name: sparepart sparepart_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sparepart
    ADD CONSTRAINT sparepart_pkey PRIMARY KEY (sparepart_id);


--
-- TOC entry 4675 (class 1259 OID 16460)
-- Name: idx_clients_phone; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_clients_phone ON public.clients USING btree (contact_phone);


--
-- TOC entry 4678 (class 1259 OID 16461)
-- Name: idx_devices_serial; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_devices_serial ON public.devices USING btree (serial_number);


--
-- TOC entry 4679 (class 1259 OID 16463)
-- Name: idx_receipts_client; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_receipts_client ON public.receipts USING btree (client_id);


--
-- TOC entry 4682 (class 1259 OID 16462)
-- Name: idx_repairs_dates; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_repairs_dates ON public.repairs USING btree (acceptance_date, completion_date);


--
-- TOC entry 4685 (class 1259 OID 16464)
-- Name: idx_sparepart_repair; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_sparepart_repair ON public.sparepart USING btree (repair_id);


--
-- TOC entry 4692 (class 2620 OID 16466)
-- Name: repairs trigger_update_repair_status; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trigger_update_repair_status BEFORE UPDATE OF completion_date, status ON public.repairs FOR EACH ROW EXECUTE FUNCTION public.update_repair_status_on_completion();


--
-- TOC entry 4688 (class 2606 OID 16415)
-- Name: receipts receipts_client_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.receipts
    ADD CONSTRAINT receipts_client_id_fkey FOREIGN KEY (client_id) REFERENCES public.clients(client_id) ON DELETE CASCADE;


--
-- TOC entry 4689 (class 2606 OID 16437)
-- Name: repairs repairs_device_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.repairs
    ADD CONSTRAINT repairs_device_id_fkey FOREIGN KEY (device_id) REFERENCES public.devices(device_id) ON DELETE CASCADE;


--
-- TOC entry 4690 (class 2606 OID 16442)
-- Name: repairs repairs_receipt_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.repairs
    ADD CONSTRAINT repairs_receipt_id_fkey FOREIGN KEY (receipt_id) REFERENCES public.receipts(receipt_id) ON DELETE CASCADE;


--
-- TOC entry 4691 (class 2606 OID 16455)
-- Name: sparepart sparepart_repair_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sparepart
    ADD CONSTRAINT sparepart_repair_id_fkey FOREIGN KEY (repair_id) REFERENCES public.repairs(repair_id) ON DELETE SET NULL;


-- Completed on 2025-10-21 21:27:46

--
-- PostgreSQL database dump complete
--

